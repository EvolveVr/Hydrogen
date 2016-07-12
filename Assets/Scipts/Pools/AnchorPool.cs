using UnityEngine;
using System.Collections;
using Hydrogen;
public class AnchorPool : ObjectPool
{

    //The amountReadyToSpawn for anchors will be set by player

    public override void initialize(int amountReadyToSpawn)
    {
        base.initialize(amountReadyToSpawn);
        //Gets the transforms of all the objects tagged with anchorspawnpoint
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("AnchorSpawnPoint");
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            //Randomizing it like this won't work cause need to add condition checking for if anchor already at same spot
            //Might re do to random but for now fuck that shizz
           // int index = Random.Range(0, spawnPoints.Length);
            _objectPool[i].transform.position = spawnPoints[i].transform.position;
            //So I need to set up empty GameObjects where Anchors would be depending on how many will be spawned. These will be tagged with AnchorSpawnPoint

        }
    }

 
   
    protected override void Awake()
    {
        _objectPrefab = Resources.Load("Prefabs/TargetPrefabs/Anchor") as GameObject;
        base.Awake();

      
        
    }
}
