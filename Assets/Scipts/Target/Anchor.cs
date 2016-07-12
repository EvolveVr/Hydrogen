using UnityEngine;
using System.Collections;

namespace Hydrogen {
    /// <summary>
    /// This script is for handling anchor movement/respawning and spawning targets
    /// </summary>
    public class Anchor : MonoBehaviour
    {
       /* private static Movement[] _movementList = new Movement[]
        {
            //So for handling deciding on movements for anchors, it will not be random but selected based on conditions of difficulty and current position
            //So for beginner difficulty this will be the function that's called.     
            () =>
            {
                Vector3 nextMovement = new Vector3();
                if (cp.x >= ip.x + md)
                {
                    nextMovement = Vector3.left;
                }
                if (cp.x <= ip.x - md)
                {
                    nextMovement = Vector3.right;
                }

                if (cp.y >= ip.y + md)
                {
                    nextMovement = Vector3.down;
                }
                if (cp.y <= ip.y - md)
                {
                    nextMovement = Vector3.up;
                }

                if (cp.z >= ip.z + md)
                {
                    nextMovement = Vector3.back;
                }
                if (cp.z <= ip.z - md)
                {
                    nextMovement = Vector3.forward;
                }

                return nextMovement;
            }
        };*/

        private GameManager _gameManager;
        //Reference to targetManager to spawn new anchor to replace when this one dies, and is also used to get random spawn point for targets
        private TargetManager _spawnNewAnchor;

        //Variables for handling anchor movement
        private NavMeshAgent _navAgent;
        private Vector3 _initPosition;
        private int chosenMovementIndex;
        /// <summary>
        /// This is list of functions for anchor movement and arguments they take in are:
        /// ip : initial position cp : current position md : max distance s : speed
        /// </summary>
        
        //List of targets to spawn
        private TargetPool _targetPool;
        //Amount of targets to spawn
        private int _amountToSpawn = 3;
        //difficulty level of the anchor
        private int _difficulty;
        private Difficulty currentDifficulty;
        //The vicinity the targets have to reside in relatitve to parent anchor
        private SphereCollider vicinity;


        //Gets needed references  
        private void Awake() 
        {
            vicinity = GameObject.FindGameObjectWithTag("Vicinity").GetComponent<SphereCollider>();
            _spawnNewAnchor = GameObject.Find("GameManager").GetComponent<TargetManager>();
            _targetPool = GameObject.Find("TargetManager").GetComponent<TargetPool>();
            _navAgent = GetComponent<NavMeshAgent>();
        }

        //Spawns targets for anchor, and changes their properties depending if this anchor is timetarget one or not
        private void Start()
        {
            _initPosition = transform.position;
            spawnPointTarget(_amountToSpawn);
            if (gameObject.tag == "Target")
            {
                //Gets all the children of the anchor
                Transform[] obstacles = new Transform[transform.childCount];
                for (int i = 0; i < transform.childCount; i++)
                {
                    obstacles[i] = transform.GetChild(i);
                }
                
                //Adds orbitTarget script to all the obstacles around our TimeTarget anchor, and spawns them in random point within the vicinity
                foreach (var x in obstacles)
                {
                    x.gameObject.AddComponent<OrbitTarget>();

                    x.localPosition = _spawnNewAnchor.getSpawnPoint(vicinity.radius);
                    x.tag = "Obstacle";
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
            if (transform.childCount < _amountToSpawn)
            {
                //As it loses targets going around it, it will change how it moves.   
                //This will be saved for harder difficulties but it should be easy, if do list way I was doing it could get index of targets current movement that died, and replace anchors
                //movement with that.
            }
        }

        /// <summary>
        /// This is for spawning point target.
        /// </summary>
        /// <param name="amountToSpawn">The amount of targets to get from pool and set active</param>
        private void spawnPointTarget(int amountToSpawn)
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                GameObject target = _targetPool.getObject();
                //Sets the anchor to parent of target.

                //This assigns the spawned target to this anchor object that this script is attached to, might randomize this like the way timeTarget is, but not sure yet
                target.transform.parent = transform;
                float anchorRadius = 0.2f;
                //Parent doesn't matter since all have same vicinity, but incase decide to have it dynamically change then need to do this
                if (vicinity.transform.parent == this.gameObject)
                {
                    anchorRadius = vicinity.radius;  
                }
                target.transform.localPosition = Vector3.zero;
                //Randomizes it's initial position within the radius of the vicinity of the anchor
                target.transform.localPosition = _spawnNewAnchor.getSpawnPoint(anchorRadius);
                //Adds the Target component script to it, since it's not time target it only needs base.
                target.AddComponent<Target>();
                target.SetActive(true);
            }
        }
       


        // public void spawnTimeTarget()
        //  {


        /*<Option1>Right now I could do two things, either put an if statement to check if any of the current targets/anchors are contain TimeTarget Componenet, or I could just spawn time targets inside the targetmanager instead, because the problem with this is, if I get reference round timer here it'll work but it will spawn a time target for each anchor.</Option1>
        <Option2>And doing in game or target manager that has reference to anchor wont work cause anchor script only exists after being spawned, which doesn't happen until player chooses amount of anchors to spawn and GameManager is active on start of game.</Option2> */


        /* GameObject target = _targetPool.getObject();

         target.transform.localPosition = new Vector3(0, 0, 0);
         target.AddComponent<TimeTarget>();
         target.SetActive(true);
         */

        // }
    }
       
}