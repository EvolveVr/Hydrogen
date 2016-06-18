using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TargetPool : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject parent;
  
    List<GameObject> targetPool;
    int _currentWave;
    int _amountToSpawn;
    void Awake()
    {

        // I think wave should be moved elsewhere, not Targetmanager, prob gamemanager whenever we make one, don't want in TargetManager because it has a refernce to
        //this script and that's circular dependancy.
        //But it works here, just mean if it makes logical sense to have it in here or not.
        _currentWave = 1;
        parent = GameObject.Find("GameObject");
        targetPool = new List<GameObject>();
     
    }
    void Start()
    {
      
        switch (_currentWave)
        {
            case 1:
                _amountToSpawn = 3;
                break;

        }
        for (int i = 0; i < _amountToSpawn; i++)
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
            //If inactive target then return that target.
            if(!targetPool[i].activeInHierarchy)
            {
                //Well this is perfect for bullets and reusing but since we have specifications on targets, should leave as is

                
                return targetPool[i];
            }
        }
        /*if all targets currently active and need to spawn more then return new one, but for targets this is unnecessary, since assuming we're going to have set number targets per wave, and not spawn new targets if they haven't killed all the ones spawned in current wave, but since not 100% sure just leaving here for now, no big deal.*/
        return Instantiate(targetPrefab);
    }
    public int CurrentWave
    {
        get { return _currentWave; }
        //set mainly for testing to do it while scene playing
        //if keeping in here could just update this when all targets killed or something.
        set { _currentWave = value; }
    }
}
