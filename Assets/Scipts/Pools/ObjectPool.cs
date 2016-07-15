using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hydrogen
{
    public class ObjectPool : MonoBehaviour
    {
        //Prefab will change depending on what we're pooling 
        public Transform poolHolder;
        protected GameObject _objectPrefab;
        protected List<GameObject> _objectPool;


        //Could create a delegate, and have that take the T generic, then have functions that do things according to type, each lambda
        //Containing an if statement for if _objectPrefab is type;

        public virtual void initialize(int amountReadyToSpawn)
        {
            //This adds all of the objects into the array
            for (int i = 0; i < amountReadyToSpawn; i++)
            {
                _objectPool.Add(Instantiate(_objectPrefab));
                _objectPool[i].SetActive(false);
                _objectPool[i].transform.parent = poolHolder;
            }
        }

        public GameObject getObject()
        {
            foreach (var x in _objectPool)
            {
                if (!x.activeInHierarchy)
                    return x;
            }

            GameObject spawned = Instantiate(_objectPrefab);
            if (spawned == null)
            {
                Debug.Log("object was never instantiated");
            }
            spawned.SetActive(false);
            _objectPool.Add(spawned);
            return spawned;
        }
        
        public void despawnAllObjects()
        {
            foreach(var x in _objectPool)
            {
                x.SetActive(false);
            }
        }
    
        protected virtual void Awake()
        {
            //instantiates the list
            _objectPool = new List<GameObject>();
        }
     


    }
}