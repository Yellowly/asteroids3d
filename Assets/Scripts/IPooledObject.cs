using UnityEngine;
public enum AsteroidTypes{
    small,
    medium,
    large
}
public delegate void runAction();
public interface IPooledObject
{
    void onObjectSpawn();
}
