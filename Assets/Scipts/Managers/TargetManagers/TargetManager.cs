using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Hydrogen
{
public class TargetManager : MonoBehaviour
{
       
    #region variables for handling movements
    /// <summary>
    /// This is a delegate that will take in functions that will return specific movement
    /// </summary>
    /// <param name="direction">This is the direction the targets are moving in, left,right,up,down etc.</param>
    /// <param name="offset">This is the initial local position of the target, that we will be updating with current direction, time, and speed we want it to be changing position.</param>
    /// <param name="axisOfMovement"> The axis the pattern will be moving on, like sin wave left right, forward back etc.</param>
    /// <param name="magnitude">This will be how big or small the waves will be</param>
    /// <param name="speedTarget">speed of the translation of the target</param>
    /// <param name ="speedMovement">speed that the target is going through the entire movement. eg: Time it takes to complete an entire sinwave
    /// </param>
    /// <returns>This returns the resulting vector3 that the targets will translate on. </returns>
    public delegate Vector3 moveFunction(Vector3 direction, Vector3 offSet, Vector3 axisOfMovement, float magnitude, float speedTarget, float speedMovement);
    private List<moveFunction> _storageMovementFunctions;
    #endregion
    //Might put move algorithims in here since moving is managing the targets, then both scrips will just have reference to this
    #region Variables managing the round
    private float _roundTimer = 30.0f;
    private float _leftOnRound;
    private bool roundOver;
    #endregion

    #region Variables managing spawn of enemies in round

        private TargetPool _targetPool;
        private int _totalAmountSpawnedAtATime;
        //The time until time targets leave field.
        private float _timeTillDeath;
   
    #endregion

    //Dictionary of movement algorithims and movefunction will be in here, but variables will be passed into arguments of those lamdas, instead of how it was before.
    public void initializeMovementFunctions()
    {
        Vector3 movement = new Vector3();
        _storageMovementFunctions.Add((Vector3 direction, Vector3 offSet, Vector3 axisOfMovement, float magnitude, float speedTarget, float speedMovement) =>
        {
            movement = offSet + axisOfMovement * Mathf.Sin(Time.time * speedMovement) * magnitude;

            return movement;

        });
        _storageMovementFunctions.Add((Vector3 direction, Vector3 offSet, Vector3 axisOfMovement, float magnitude, float speedTarget, float speedMovement) =>
        {
            movement.x =  Mathf.Sin(Time.time * speedMovement) * magnitude;
            movement.y =  Mathf.Cos(Time.time * speedMovement) * magnitude;

            return movement;
                
        });


    }
    public moveFunction getMovement()
    {

        //Movement they start with will be random but they will keep that movement
        int index = Random.Range(0, _storageMovementFunctions.Count);

        moveFunction movementIndexed = _storageMovementFunctions[index];
        
        if (movementIndexed == null)
        {
            return _storageMovementFunctions[index - 1];
        }

        return movementIndexed;
    }


    public int totalAmountSpawnedAtATime
    {
        get { return _totalAmountSpawnedAtATime;
        }
        set { _totalAmountSpawnedAtATime = value; }
    }

   
    
    public void callSpawner(string spawnType, int amountToSpawn = 0, bool obstacle = false)
    {
                switch (spawnType)
                {
                    case "point":
                        StartCoroutine(spawnPointTarget(amountToSpawn));
                        break;
                    case "time":
                        StartCoroutine(spawnTimeTarget(obstacle));
                        break;
                }
    }
    private IEnumerator spawnPointTarget(int amountToSpawn)
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < amountToSpawn; i++)
        {
           
            GameObject target = _targetPool.getObject();
            //Sets position to spawn point
            //This will be initial spawn point, then local position relative to parent anchor will change on the PointTarget script attached to 
            //target object.
            float lowerRange = 0.1f;
            float endRange = 5.7f;
            target.transform.localPosition = new Vector3(0, 0, 0);
            target.AddComponent<PointTarget>();
            target.SetActive(true);
           
        
        }
        
    }
   
    private IEnumerator spawnTimeTarget(bool obstacle)
    {
        //This seperate thread so spawn time target will auto kill the spawned timetarget after a given certain amount of time
        //Or unless it is shot, which will be handled on TimeTarget script attached to timeTarget Object
        GameObject target;

        if (obstacle)
        {
            //So if obstacle == true then the the target will be the anchor
            GameObject anchorPrefab = Resources.Load("Prefabs/TargetPrefabs/Anchor") as GameObject;
            target = Instantiate(anchorPrefab);
            target.tag = "Obstacle";


        }
        else
        {
            target = _targetPool.getObject();
        }
        target.transform.localPosition = new Vector3(0, 0, 0);
        target.AddComponent<TimeTarget>();

        //   target.GetComponent<TimeTarget>().timeGainedFromTarget = timeTillDeath;
        target.SetActive(true);
        yield return new WaitForSeconds(_timeTillDeath);
        TimeTarget component = target.GetComponent<TimeTarget>();
        Destroy(component);
        target.SetActive(false);

    }
    

    private void Awake()
    {
        _storageMovementFunctions = new List<moveFunction>();
        _targetPool = gameObject.GetComponentInChildren<TargetPool>();
    }

 
	private void Start()
    {
        
        _timeTillDeath = 5.0f;
        initializeMovementFunctions();
        _targetPool.initialize(totalAmountSpawnedAtATime);
        StartCoroutine(spawnPointTarget(totalAmountSpawnedAtATime/2));
      
    }



}
}