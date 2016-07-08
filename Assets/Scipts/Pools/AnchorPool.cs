using UnityEngine;
using System.Collections;
using Hydrogen;
public class AnchorPool : ObjectPool
{

    //The amountReadyToSpawn for anchors will be set by player

    public override void initialize(int amountReadyToSpawn)
    {
        base.initialize(amountReadyToSpawn);
        for (int i = 0; i < _objectPool.Count; i++)
        {
            //So I need to set up empty GameObjects where Anchors would be depending on how many will be spawned. These will be tagged with spawnPointN
            _objectPool[i].transform.position = GameObject.FindGameObjectWithTag("spawnPoint" + i).transform.position;
        }
    }

 
    private void Awake()
    {
        _objectPrefab = Resources.Load("Prefabs/AnchorPrefabs/anchor") as GameObject;
        
    }
}
