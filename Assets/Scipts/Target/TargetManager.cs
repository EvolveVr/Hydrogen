using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TargetManager : MonoBehaviour
{
    #region Variables managing the round
    private float _roundTimer = 30.0f;
    private float _leftOnRound;
    private bool roundOver;
    #endregion

    #region Variables managing spawn of enemies in round

    private GameManager _manageSpawnCount;
    private TargetPool _targetPool;
    private float _timeTillNextWaveSpawns = 5.0f;
    private float _leftTillWaveSpawns;
    private int _amountToSpawn;
    private float _spawnTimeInterval;
    #endregion

    /// <summary>
    /// Sets the amount of targets to spawn
    /// Returns the amount set
    /// </summary>
    public int amountToSpawn
    {
        get { return _amountToSpawn; }
        set { _amountToSpawn = value; }
    }
   
    /// <summary>
    /// This spawns the targets and handles
    /// all of their properties before setting them to active
    /// </summary>
    /// <returns>A short timer between each individual spawn</returns>
    public IEnumerator spawnTarget()
    {
        //Grabs object from pool
        for (int i = 0; i < _amountToSpawn; i++)
        {
            //Gets pooled object
            GameObject target = _targetPool.getTarget();
            //Sets position to spawn point
            target.transform.position = GameObject.FindGameObjectWithTag("TargetSpawnPoint").GetComponent<Transform>().position;
            target.transform.localScale = new Vector3(2, 2, 2);
            target.SetActive(true);
            //waits for spawnTimeInterval
            yield return new WaitForSeconds(_spawnTimeInterval);
        }
    }

    private void Awake()
    { 
        
        _manageSpawnCount = gameObject.GetComponent<GameManager>();
        _targetPool = gameObject.GetComponentInChildren<TargetPool>();
    }

    /// <summary>
    /// Sets the round being over to false.
    /// Sets all of the timers.
    /// </summary>
	private void Start()
    {
         roundOver = false;

        _leftOnRound = _roundTimer;
        _leftTillWaveSpawns = _timeTillNextWaveSpawns;
        _spawnTimeInterval = 0.3f;
    }
    
    /// <summary>
    /// This function just decreases all of the timers
    /// and executes what they need to depending on total timer interval
    /// differentiating what the timer is for.
    /// </summary>
    /// <param name="currentTime">The timer that will be decrementing</param>
    /// <param name="maxTime">The total time it starts from and will reset to</param>
    private void decreaseTimer(ref float currentTime, float maxTime)
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else if (currentTime <= 0)
        {
            if (maxTime == _timeTillNextWaveSpawns)
            {
                _manageSpawnCount.currentWave = 1;

                currentTime = maxTime;
            }
            else if (maxTime == _roundTimer)
            {
                _targetPool.despawnAllTargets();
                Debug.Log("You've earned " + _manageSpawnCount.playerPoints + " points");

                roundOver = true;
            }
        }

    } 

    private void Update()
    {
        //Debugging purposes
        if (Input.GetKeyDown(KeyCode.A))
        {

            _manageSpawnCount.currentWave = 1;

        }

        if (!roundOver)
        {
            decreaseTimer(ref _leftOnRound, _roundTimer);
            decreaseTimer(ref _leftTillWaveSpawns, _timeTillNextWaveSpawns);
        }
    }
   
}
