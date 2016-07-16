using UnityEngine;
using System.Collections;
/// <summary>

/// </summary>
namespace Hydrogen
{
    public class TargetPool : ObjectPool
    {
        public override void initialize(int amountReadyToSpawn)
        {
            base.initialize(amountReadyToSpawn);
          
            for (int i = 0; i < _objectPool.Count; i++)
            {
                //Spawn points will be randomized but within the vicinity of the anchor they are attached to
                _objectPool[i].transform.parent = poolHolder;
                _objectPool[i].transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);


            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            //Only 5 on screen at a time, or on anchor at a time
            _objectPrefab = Resources.Load("Prefabs/TargetPrefabs/Target") as GameObject;
        }
        

    }
}