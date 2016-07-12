using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hydrogen
{
    /// <summary>
    /// This will handle spawning anchors and time targets 
    /// </summary>
    public class TargetManager : MonoBehaviour
    {

        #region Variables managing spawn of enemies in round

        private AnchorPool _anchorPool;
        private TargetPool _targetPool;
        private int _totalAmountSpawnedAtATime;
       

        #endregion


        //When round is over GameManager will call this to despawn all of the enemies
        public void despawnAllAnchors()
        {
            _anchorPool.despawnAllObjects();

        }

        /// <summary>
        /// This represents total amount of anchors spawed at a time, which will be chosen by the player at the start of the game.
        /// </summary>
        public int totalAmountSpawnedAtATime
        {
            get
            {
                return _totalAmountSpawnedAtATime;
            }
            set { _totalAmountSpawnedAtATime = value; }
        }
   
        /// <summary>
        /// In here is to set the position of the targets randomly but within vicinity of respective anchor
        /// </summary>
        /// <param name="endPoint">What will be passed in is radius and - and postive versions will be end points of the Random.Range</param>
        /// <returns></returns>
        public Vector3 getSpawnPoint(float endPoint)
        {
            //Thought about having two arguments, so it's more general but really only need for spawn points and within radius is best so this is fine
            Vector3 spawnPoint = new Vector3(Random.Range(-endPoint, endPoint), Random.Range(-endPoint, endPoint), Random.Range(-endPoint, endPoint));

            return spawnPoint;
        }
     
        /// <summary>
        /// This function spawns the time target where anchor is target and targets around it are now obstacles
        /// </summary>
        public void spawnTimeTargetAnchor()
        {

            GameObject targetAnchor = _anchorPool.getObject();
            targetAnchor.tag = "Target";
            Anchor component = targetAnchor.GetComponent<Anchor>();
          //Destroy(component);
            targetAnchor.SetActive(true);
            targetAnchor.AddComponent<TimeTarget>();
        }

        /// <summary>
        /// This is called at time points in round timer to spawn target that disappears after certain amount of time
        /// and if killed via being shot, the player wll add time to the round timer.
        /// </summary>
        public void spawnTimeTarget()
        {
            GameObject target = _targetPool.getObject();
        
            //This returns all anchors in the scene into an array
            GameObject[] allAnchors = GameObject.FindGameObjectsWithTag("Anchor");
            //This randomizes which anchor we will parent this target to
            int randomIndex = Random.Range(0, allAnchors.Length);
            //Checks if anchor is active before setting the parent of this target to it,
            if (allAnchors[randomIndex].activeInHierarchy)
                target.transform.parent = allAnchors[randomIndex].transform;
            //This gets the radius of the anchor.
            float anchorRadius = allAnchors[randomIndex].GetComponent<SphereCollider>().radius;
            target.transform.localPosition = getSpawnPoint(anchorRadius);
        
            //This adds the TimeTarget script to the target.
            target.AddComponent<TimeTarget>();
            //This sets the target to active.
            target.SetActive(true);


        }

        /// <summary>
        /// This spawns the anchors from their pool
        /// </summary>
        /// <param name="amountToSpawn">The initial value passed in will be player choice of how many anchors they want at a time and all other times will be to replace when an
        /// anchor dies </param>
        public void spawnAnchor(int amountToSpawn)
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                GameObject anchor = _anchorPool.getObject();
                anchor.SetActive(true);
            }

        }

        //Gets references to anchor and target pools
        private void Awake()
        {

            _targetPool = gameObject.GetComponentInChildren<TargetPool>();
            _anchorPool = gameObject.GetComponentInChildren<AnchorPool>();
        }

        private void Start()
        {
            //All of this is only temporarily on start will be on initialize function that GameManager will call during actual game because need to wait for player input for
            //totalAmountSpawnedAtATime

            //It should be set up so that it waits for UI input
            totalAmountSpawnedAtATime = 5;
            //This initializes pool to hold 6 anchors inactive, 6th is the anchor that is a timed target.
            _anchorPool.initialize(totalAmountSpawnedAtATime+1);
            spawnAnchor(totalAmountSpawnedAtATime);
            //This initializes pool to hold the amount of total anchors on at once * 6 because 6 will be max per anchor, maybe more
            //But if less then can take less, but this makes it so atleast 6 each will be possible.
            _targetPool.initialize(totalAmountSpawnedAtATime * 6);

        }

    }
}