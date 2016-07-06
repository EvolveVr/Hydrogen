
﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// This script is for creating a list of targets at run time.
/// This way when it creates alot of targets, it won't lag out cause of dynamically allocating a bunch of memory at once.
/// Instead we allocate a bunch of memory at start, then just reuse those same instances, setting active to false instead of destroying targets.
/// </summary>
public class TargetPool : MonoBehaviour
{
    #region Variables used to create target pool.
    public GameObject targetPrefab;
    //The object holding all the targets, so it doesn't clog heirarchy
    public Transform poolHolder;
    //List for the instantiated targets
    private List<GameObject> _targetPool;
    //The amount of objects we're pooling
    private int _amountReadyToSpawn;
    #endregion

    /// <summary>
    /// This function checks all of pool to see if any are
    /// still inactive, and if any are then it will return that object.
    /// If not it will create a new object and return that instead.
    /// </summary>
    /// <returns> Target object that was instantiated from prefab</returns>
    public GameObject getTarget()
    {
        foreach (var x in _targetPool)
        { 
            if (!x.activeInHierarchy)
                return x;
      
        }
     
        GameObject bullet = Instantiate(targetPrefab);
        bullet.transform.parent = poolHolder;
        if (bullet == null)
        {
            Debug.Log("bullet was never instanced");
        }
        return bullet;
    }

    /// <summary>
    /// This will be called when round timer hits zero, it will look for all objects in scene
    /// with the tag target and despawns them. If within pool, set inactive for reuse in later rounds,
    /// else, destroy them
    /// </summary>
    public void despawnAllTargets()
    {
        GameObject[] targetsSpawned = GameObject.FindGameObjectsWithTag("target");
        foreach (var x in targetsSpawned)
        {
            //Destroys objects that aren't inside the pool
            if (x.transform.parent != poolHolder)
                Destroy(x);
            else
                x.SetActive(false);

        }
    }

    //This intantiates the list of gameobjects
    private void Awake()
    {
        _targetPool = new List<GameObject>();
     
    }

    /// <summary>
    /// initializes the pool with instances of prefab
    /// and sets the parent for each object to empty game object in scene
    /// </summary>
    private void Start()
    {
        _amountReadyToSpawn = 20;
            
        for (int i = 0; i < _amountReadyToSpawn; i++)
        {
            _targetPool.Add(Instantiate(targetPrefab));
            _targetPool[i].SetActive(false);
            _targetPool[i].transform.parent = poolHolder.transform;
        }


    }
    
}
