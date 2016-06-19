using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hydrogen;
public abstract class BulletPool : MonoBehaviour
{
    //Put the prefab of right bullet in derived scripts.
    public GameObject bulletPreFab;
    //Empty gameobject
    protected Transform parent;
    protected List<GameObject> bulletPool;
    //Need magazine reference to get max bullet count for each type of gun.
    protected Magazine currentMagazine;
   

    //In awake cause the bulletManager was getting the pool before it was instantiated
    protected virtual void Awake()
    {
       //This instantiates the pool.
        bulletPool = new List<GameObject>();

    }
    
    //This initializes the pool
   protected void initialize()
    {
 
       for (int i = 0; i < currentMagazine.maxBulletCount; i++)
        {
            
            bulletPool.Add(Instantiate(bulletPreFab));
    
            //Setting  all spawned bullets to inactive.
            bulletPool[i].SetActive(false);

            //This is parenting it to an empty game object so the bullets don't clog heirarchy
            bulletPool[i].transform.parent = parent;
        }
    }
    //This returns the pool, so I can add each one to dictionary
    public List<GameObject> getPool
    {
        get
        {
            return bulletPool;
        }

    }

}