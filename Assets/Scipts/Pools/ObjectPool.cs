using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ObjectPool: MonoBehaviour
{
    //Prefab will change depending on what we're pooling
    protected GameObject _objectPrefab;
    protected List<GameObject> _objectPool;

    //Could create a delegate, and have that take the T generic, then have functions that do things according to type, each lambda
    //Containing an if statement for if _objectPrefab is type;

    public GameObject getObject()
    {
        foreach (var x in _objectPool)
        {
             if (!x.activeInHierarchy)
                  return x;
        }
        Debug.Log(_objectPrefab.GetType());
        //This saysit returns Unity.GameObject so wtf.

        //T spawned = Instantiate(_objectPrefab) as _objectPrefab.GetType();
        GameObject spawned = Instantiate(_objectPrefab);
        if (spawned == null)
        {
            Debug.Log("object was never instantiated");
        }
        _objectPool.Add(spawned);
        return spawned;
    }
    
    protected virtual void Awake()
    {
        //instantiates the list
        _objectPool = new List<GameObject>();
    }
	public virtual void initialize(int amountReadyToSpawn)
    {
        //Don't want to put it in start, actually for same issue as why used _currentWave directly
        for (int i = 0; i < amountReadyToSpawn; i++)
        {
            _objectPool.Add(Instantiate(_objectPrefab));
        }
	}
	

}
