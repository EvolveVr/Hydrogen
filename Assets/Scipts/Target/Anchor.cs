using UnityEngine;
using System.Collections;

namespace Hydrogen {
    /// <summary>
    /// This script is for handling anchor movement/respawning and spawning targets
    /// </summary>
    public class Anchor : MonoBehaviour
    {
      

        private GameManager _gameManager;
        //Reference to targetManager to spawn new anchor to replace when this one dies, and is also used to get random spawn point for targets
        private TargetManager _spawnNewAnchor;

        //Variables for handling anchor movement
        private NavMeshAgent _navAgent;
        private Vector3 _initPosition;
        private float _anchorSpeed;
        
        //List of targets to spawn
        private TargetPool _targetPool;
        //Amount of targets to spawn
        private int amountToSpawn;
        float minDistance;
        float maxDistance;
       
        //The vicinity the targets have to reside in relatitve to parent anchor
        private SphereCollider vicinity;


        

        /// <summary>
        /// This is for spawning point target.
        /// </summary>
        /// <param name="amountToSpawn">The amount of targets to get from pool and set active</param>
        private IEnumerator spawnPointTarget(int amountToSpawn)
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                
                GameObject target = _targetPool.getObject();
                //Sets the anchor to parent of target.

                //This assigns the spawned target to this anchor object that this script is attached to, might randomize this like the way timeTarget is, but not sure yet
                target.transform.parent = transform;
                //Parent doesn't matter since all have same vicinity, but incase decide to have it dynamically change then need to do this

                float anchorRadius = vicinity.radius;


                target.transform.localPosition = Vector3.zero;
                //Randomizes it's initial position within the radius of the vicinity of the anchor
                target.transform.localPosition += _spawnNewAnchor.getSpawnPoint(minDistance * Time.deltaTime, maxDistance * Time.deltaTime);
                //Adds the Target component script to it, since it's not time target it only needs base.
                target.AddComponent<Target>();
                target.AddComponent<OrbitTarget>();
                Target setStats = target.GetComponent<Target>();
                if (_gameManager.currentDifficulty == "easy")

                    //If easy then do this specific pattern, I have to do things a bit differently, But basically I want 2 of the targets to go up and to of the targets to move left and right, and some else depending on location of their anchor
                    setStats.setPatternVars(10.0f, 20.0f);
                if (_gameManager.currentDifficulty == "medium")
                    setStats.setPatternVars(3.5f, 2.5f);
                if (_gameManager.currentDifficulty == "hard")
                    setStats.setPatternVars(anchorRadius, 3.5f);
                target.SetActive(true);
                //Time interval between each spawn to make sur eincase they random to same spot, they won't ever be on top of each other.
                yield return new WaitForSeconds(0.5f);
            }
        }
        //Gets needed references  
        private void Awake()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            vicinity = GameObject.FindGameObjectWithTag("Vicinity").GetComponent<SphereCollider>();
            _spawnNewAnchor = GameObject.Find("GameManager").GetComponent<TargetManager>();
            _targetPool = GameObject.Find("TargetManager").GetComponent<TargetPool>();
        }

        //Spawns targets for anchor, and changes their properties depending if this anchor is timetarget one or not
        private void Start()
        {
            minDistance = 3.0f;
            maxDistance = 4.5f;
            //numbers set not perm.
            if (_gameManager.currentDifficulty == "easy")
                amountToSpawn = 3;
            if (_gameManager.currentDifficulty == "medium")
                amountToSpawn = 5;
            if (_gameManager.currentDifficulty == "hard")
                amountToSpawn = 6;

            StartCoroutine(spawnPointTarget(amountToSpawn));
            //If the anchor being spawned is time target anchor.
            if (gameObject.tag == "Target")
            {
                //I kinda want to spawn this one more often.
                //Gets all the children of the anchor
                Transform[] obstacles = new Transform[transform.childCount];
                for (int i = 0; i < transform.childCount; i++)
                {
                    obstacles[i] = transform.GetChild(i);
                }

                //Adds orbitTarget script to all the obstacles around our TimeTarget anchor, and spawns them in random point within the vicinity
                foreach (var x in obstacles)
                {
                    if (x.tag != "Vicinity")
                    {
                        x.gameObject.AddComponent<OrbitTarget>();

                        x.localPosition = _spawnNewAnchor.getSpawnPoint(vicinity.radius / 2, vicinity.radius);
                        x.tag = "Obstacle";
                    }
                }
            }
        }


        private void Update()
        {
            //When it no longer has targets around it it will die and then we spawn a new one to replace it, the remainging child is the vicinity
            if (transform.childCount == 1)
            {
                _spawnNewAnchor.spawnAnchor(1);
                gameObject.SetActive(false);
            }
            if (transform.childCount < amountToSpawn)
            {
                if (transform.childCount - amountToSpawn == amountToSpawn / 2)
                {
                    _anchorSpeed *= 2;
                    //Might change vicinity size too and then have them start to orbit.
                }


            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Target")
            {
                
            }

        }

    }
       
}