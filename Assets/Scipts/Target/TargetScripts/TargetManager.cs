using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TargetManager : MonoBehaviour
{

    TargetPool _targetPool;
    //current wave, here temporarily, normally will grab from gameManager.
    public int _currentWave;
    float _spawnInterval;
    void Awake()
    {
        
        _targetPool = gameObject.GetComponent<TargetPool>();
    }
	void Start()
    {
        //In future will get current wave from gamemanager
        _currentWave = 1;
        if(_currentWave == 1)
        {
            //the amount of pooled objects to set active will depend on currentwave
            amountToSpawn = 3;
        }
        //The time it takes to spawn each object
        _spawnInterval = 0.4f;
        //StartsCoroutine to spawn targets in intervals
        StartCoroutine(spawnTarget());
       
    }
    public int amountToSpawn
    {
        //Sets amount to grab from pool and spawn, depending on current wave
        get; set;
    }
  
    IEnumerator spawnTarget()
    {
        //Grabs object from pool
        for (int i = 0; i < amountToSpawn; i++)
        {
            //Gets pooled object
            GameObject target = _targetPool.getTarget();
            //Sets position to spawn point
            target.transform.position = GameObject.Find("spawnPoint").GetComponent<Transform>().position;
            //Spawns the the pooled object onto game screen
            target.SetActive(true);
            //interval between each target is spawned
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
