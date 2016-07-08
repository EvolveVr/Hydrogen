using UnityEngine;
using System.Collections;
/// <summary>
/// This script is for creating a list of targets at run time.
/// This way when it creates alot of targets, it won't lag out cause of dynamically allocating a bunch of memory at once.
/// Instead we allocate a bunch of memory at start, then just reuse those same instances, setting active to false instead of destroying targets.
/// </summary>
namespace Hydrogen
{
    public class TargetPool : ObjectPool
    {
        //Need to change parent  of this to anchor and seperate the ball targets from orb

        public Transform anchorHolder;

        public int getNumberTargetsSpawned()
        {
            int amountActive = -1;
            foreach (var x in _objectPool)
            {
                if (x.activeInHierarchy) ++amountActive;
            }
            Debug.Log(amountActive);
            return amountActive;
        }

        public void despawnAllTargets()
        {
            GameObject[] targetsSpawned = GameObject.FindGameObjectsWithTag("Target");
            foreach (var x in targetsSpawned)
            {
                //Destroys objects that aren't inside the pool

                //This way it despawns both anchors and targets at same time
                x.SetActive(false);

            }
        }

        //This intantiates the list of gameobjects
        protected override void Awake()
        {
            base.Awake();
            //Only 5 on screen at a time, or on anchor at a time
            _objectPrefab = Resources.Load("Prefabs/TargetPrefabs/orbTarget") as GameObject;
        }
        public override void initialize(int amountReadyToSpawn)
        {
            base.initialize(amountReadyToSpawn);
            for (int i = 0; i < _objectPool.Count; i++)
            {
                // _objectPool[i].SetActive(false);
                //Spawn points will be randomized but within the vicinity of the anchor they are attached to
                _objectPool[i].transform.parent = anchorHolder;
                _objectPool[i].SetActive(false);


            }
        }

    }
}