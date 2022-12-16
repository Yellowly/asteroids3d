using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IPooledObject
{
    public AsteroidTypes type;
    public Vector3 direction;
    public float speed;
    public Transform ship;
    public ParticleSystem explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void onObjectSpawn()
    {
        direction = Random.onUnitSphere;
        if (ship == null)
        {
            ship = ShipController.Instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        wrapPosition();

        //uncomment for direction and distance to ship vectors
        //Debug.DrawLine(transform.position, transform.position + direction * 10f);
        Debug.DrawLine(transform.position, ship.position, Vector3.Distance(transform.position,ship.position)>Camera.main.farClipPlane?Color.red:Color.blue);
    }

    void wrapPosition()
    { 
        //using if statements would work just as well here but i just think this format looks cool
        transform.position = Mathf.Abs(transform.position.x - ship.position.x) > Camera.main.farClipPlane+10 ? transform.position + Vector3.right * Mathf.Sign(ship.position.x - transform.position.x) * (Camera.main.farClipPlane+10)*2 : transform.position;
        transform.position = Mathf.Abs(transform.position.y - ship.position.y) > Camera.main.farClipPlane + 10 ? transform.position + Vector3.up * Mathf.Sign(ship.position.y - transform.position.y) * (Camera.main.farClipPlane + 10) * 2 : transform.position;
        transform.position = Mathf.Abs(transform.position.z - ship.position.z) > Camera.main.farClipPlane + 10 ? transform.position + Vector3.forward * Mathf.Sign(ship.position.z - transform.position.z) * (Camera.main.farClipPlane + 10) * 2 : transform.position;
    }

    public void setupType(AsteroidTypes type)
    {
        this.type = type;
        switch (type)
        {
            case AsteroidTypes.small:
                speed = 8f;
                transform.localScale = Vector3.one;
                break;
            case AsteroidTypes.medium:
                speed = 6f;
                transform.localScale = Vector3.one * 1.8f;
                break;
            case AsteroidTypes.large:
                speed = 4f;
                transform.localScale = Vector3.one * 2.5f;
                break;
        }
    }
    public void destroyAsteroid()
    {
        Instantiate(explosion, transform.position,Quaternion.identity);
        AsteroidManager.points += AsteroidManager.Instance.pointsPerAsteroidType[2-(int)type];
        if (type == AsteroidTypes.large)
        {
            for (int i=0; i <= 1; i++)
            {
                AsteroidManager.Instance.spawnAsteroid(transform.position+Random.onUnitSphere * 2, Quaternion.identity, AsteroidTypes.medium);
            }
            gameObject.SetActive(false);
        } else if (type==AsteroidTypes.medium)
        {
            for (int i = 0; i <= 1; i++)
            {
                AsteroidManager.Instance.spawnAsteroid(transform.position + Random.onUnitSphere * 2, Quaternion.identity, AsteroidTypes.small);
            }
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }
}
