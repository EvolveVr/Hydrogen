using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TargetPool : MonoBehaviour
{
    public GameObject targetPrefab;
    //The object holding all the targets, so it doesn't clog heirarchy
    public GameObject parent;
  
    //List for the instantiated targets
    List<GameObject> targetPool;
   
    //The amount of objects we're pooling
    int _amountReadyToSpawn;

    void Awake()
    {
        parent = GameObject.Find("GameManager");
        targetPool = new List<GameObject>();
     
    }
    void Start()
    {
        //Just a placeholder amount rn for size of pool itself.
            _amountReadyToSpawn = 20;
            
        for (int i = 0; i < _amountReadyToSpawn; i++)
        {
            targetPool.Add(Instantiate(targetPrefab));
            targetPool[i].SetActive(false);
            targetPool[i].transform.parent = parent.transform;

        }


    }

    public GameObject getTarget()
    {
        for (int i = 0; i < targetPool.Count; i++)
        {
            //If inactive target then return that target, so we can reuse a dead one
            if(!targetPool[i].activeInHierarchy)
            {
               
                
                return targetPool[i];
            }
        }
        //If all active and more need to be spawned we'll create a new one
        return Instantiate(targetPrefab);
    }
 
}
