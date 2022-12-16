using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    public float speed;
    private float timer;
    public float length;

    // Start is called before the first frame update
    public void onObjectSpawn() //replacement for the Start() method for use with object pool
    {
        timer = length;
        //something should go here but i forget what
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; //bullet kills itself after 20 seconds
        transform.position += transform.forward * speed*Time.deltaTime; //moves bullet
        //raycast collision detection. i don't like using unity's built in collision, especially for projectials, where it's a lot more preformant to just do a raycast
        RaycastHit ray = new RaycastHit();
        Physics.Raycast(transform.position + transform.forward, transform.forward,out ray,speed); 
        if (ray.collider != null)
        {
            if (ray.collider.tag == "asteroid" && ray.distance <= 0.5) 
            {
                ray.collider.GetComponent<Asteroid>().destroyAsteroid();
                //ray.collider.gameObject.SetActive(false);

                gameObject.SetActive(false);
            }
        }
        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
        
    }

}
