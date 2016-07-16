using UnityEngine;
using System.Collections;
using Hydrogen;
public class bulletPool : ObjectPool {

    public Transform firePosition;
	// Use this for initialization
	protected override void Awake()
    {
        base.Awake();
        _objectPrefab = Resources.Load("Prefabs/Bullet/BulletPrefabs") as GameObject;
	
	}

    public override void initialize(int amountReadyToSpawn)
    {
        base.initialize(amountReadyToSpawn);
        for (int i = 0; i < _objectPool.Count; i++)
        {
            // _objectPool[i].SetActive(false);
            //Spawn points will be randomized but within the vicinity of the anchor they are attached to
            
            _objectPool[i].SetActive(false);
            _objectPool[i].transform.position = firePosition.position + Vector3.forward;
            //Setting position should probably be set in manager not pool
            //_objectPool[i].transform.position = GameObject.Find("spawnPoint" + i).transform.position;
        }
    }
}
