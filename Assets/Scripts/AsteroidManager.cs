using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public float[] asteroidsPerLevel;
    public float[] pointsPerAsteroidType;
    public static float points = 0;
    public Text point;
    public int currentLevel;
    public Transform ship;
    public static AsteroidManager Instance;
    public void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        //this will go into a "OnRestart" function once that becomes a thing
        for (int i = 0; i < asteroidsPerLevel[currentLevel - 1]; i++)
        {
            spawnAsteroid(ship.transform.position + Random.insideUnitSphere * Camera.main.farClipPlane, Quaternion.identity, AsteroidTypes.large);
        }
    }
    private void Update()
    {
        point.text = points.ToString();
        setupLevel();
    }
    void setupLevel()
    {
        if (points >= asteroidsPerLevel[currentLevel-1] * 520)
        {
            currentLevel += 1;
            for (int i = 0; i < asteroidsPerLevel[currentLevel - 1]; i++)
            {
                spawnAsteroid(ship.transform.position+Random.insideUnitSphere * Camera.main.farClipPlane, Quaternion.identity, AsteroidTypes.large);
            }
        }
    }

    public void spawnAsteroid(Vector3 pos, Quaternion rotation, AsteroidTypes type)
    {
        GameObject asteroidInstance;
        asteroidInstance = ObjectPooler.Instance.SpawnFromPool("asteroid", pos, rotation);
        asteroidInstance.GetComponent<Asteroid>().setupType(type);
    }
}
