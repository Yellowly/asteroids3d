using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float maxThrust;
    private float thrust;
    public Vector3 turnTorque;
    public float forceMultiplier;
    public float sens; //sensitivity 
    public float bankAngle;
    public MouseController controller;
    private float yaw, pitch, roll;
    Rigidbody rb;
    private float shootTimer;
    public float shootCooldown;
    int lives = 5;
    public bool invunerable;
    public Counter livesCounter;
    public ParticleSystem explosion;
    bool canInput=true;
    public AudioSource pewpew;
    public AudioSource vroom;


    public static Transform Instance;
    private void Awake()
    {
        Instance = GetComponent<Transform>();
    }

    void Start()
    {
        canInput = true;
        rb = GetComponent<Rigidbody>();
    }

    void Update() 
    {
        if (canInput)
        {
            turnToTarget(controller.pointer, ref yaw, ref pitch, ref roll);
            moveShip();
            Shoot();
        }
    }

    private void FixedUpdate() //forces always go in fixed update
    {
        rb.AddRelativeForce(Vector3.forward * thrust * forceMultiplier, ForceMode.Force);
        rb.AddRelativeTorque(torque,ForceMode.Force);
    }

    public Vector3 torque //i think this is self explanitory 
    {
        get
        {
            return new Vector3(turnTorque.x * pitch*forceMultiplier, turnTorque.y * yaw*forceMultiplier, -turnTorque.z * roll*forceMultiplier);
        }
    }

    void moveShip()
    {
        //adds a thrust force to the ship based on pressing W or S
        thrust = Input.GetAxis("Vertical") * maxThrust;
        if (Input.GetAxisRaw("Vertical")!=0){
            if (!vroom.isPlaying)
            {
                vroom.Play();
            }
        }
        else
        {
            vroom.Stop();
        }

        //"I'll try spinning - that's a good trick!"
        roll = Input.GetButton("Horizontal")? Input.GetAxis("Horizontal") * turnTorque.z*0.8f:roll;
    }

    //supposed to be for an airplane, but looks cool in this game as well. having a ship that just rotates over 1 axis, while more realistic, is boring.
    //this will bank the ship towards the target before turning. it relys on the fact that most planes have better roll and pitch control than yaw control.
    void turnToTarget(Vector3 target, ref float yaw, ref float pitch, ref float roll) //might use ref instead of out idk edit: i did in fact do this
    {
        Vector3 localTarget = transform.InverseTransformPoint(target).normalized * sens; //turns target world pos into a local position relative to ship's rotation and position
        float angle = Vector3.Angle(transform.forward, target - transform.position); 
        //i bet there's a better way to do this somewhere

        yaw = Mathf.Clamp(localTarget.x, -1, 1);
        pitch = -Mathf.Clamp(localTarget.y, -1, 1);
        float rollAngle = Mathf.Clamp(localTarget.x, -1, 1); 
        float wingLevel = transform.right.y;

        roll = Mathf.Lerp(wingLevel, rollAngle, Mathf.InverseLerp(0, bankAngle, angle)); //smoothly interpolate roll by incrementing from angle off target
    }

    void Shoot() //pew pew
    {
        shootTimer = shootTimer > 0f? shootTimer - Time.deltaTime : 0f;
        if (Input.GetMouseButtonDown(0)&&shootTimer==0)
        {
            //pew pew!
            pewpew.Play();

            //find the correct rotation for the bullet before spawning.
            //Quaternion bulletRotation = controller.cursorTarget.collider != null ? Quaternion.LookRotation((controller.cursorTarget.point-transform.position).normalized) : transform.rotation;

            //bullet rotation was buggy so instead of fixing the problem i just ignored it
            Quaternion bulletRotation = transform.rotation;

            //spawns bullet
            ObjectPooler.Instance.SpawnFromPool("Bullet", transform.position+transform.forward*2, bulletRotation);

            //cooldown for shooting
            shootTimer += shootCooldown;

            //uncomment for visible aiming line
            Debug.DrawLine(transform.position + transform.forward * 2, controller.cursorTarget.point,Color.red);

        }
    }
    void onDeath()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        lives -= 1;
        livesCounter.setActiveImages(lives);
        canInput = false;
        rb.velocity=Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        thrust = 0;
        GetComponent<Collider>().enabled = false;
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = false;
        }
        GetComponentInChildren<TrailRenderer>().emitting =false;
        StartCoroutine(respawn());

    }
    IEnumerator respawn()
    {
        yield return new WaitForSeconds(2);
        transform.position = Vector3.zero;
        GetComponentInChildren<TrailRenderer>().emitting = true;
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = true;
        }
        canInput = true;
        yield return new WaitForSeconds(4);
        GetComponent<Collider>().enabled = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "asteroid"&&invunerable==false)
        {
            onDeath();
            
        }
    }


}
