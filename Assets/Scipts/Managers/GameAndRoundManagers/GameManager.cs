using UnityEngine;
using System.Collections;
/// <summary>
/// This script handles amount of enemies to spawn depending on currentWave which is updated
/// by TargetManager, might switch that around since think should actually be other way around
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    private TargetManager _manageTargets;
    //This variable will be used to set amount to spawn and call the spawnTarget function
    protected Vector3 spawnPosition;
    protected int _maxSpawnAtATime;

    #region Variables managing the round
    private float _roundTimer = 60.0f;
    public float _timeLeftInRound;
    private bool roundOver;
    #endregion

    private int _playerPoints;
    
    //  public abstract IEnumerator spawnPointTarget(int amountToSpawn);
    public float timeLeftInRound
    {
        set { _timeLeftInRound -= value; }
        //return time left to keep track of in UI
        get { return _timeLeftInRound; }
    }
    public float roundTimer
    {
        set { _roundTimer = value; }
        get { return _roundTimer; }
    }

    /// <summary>
    /// This adds to player points and returns the tallied up score when round is over.
    /// </summary>
    public int playerPoints
    {
        set { _playerPoints += value; }
        get { return _playerPoints; }//Returning for UI
    }

    //Adds time left in round when player kills TimeTarget
    public float addTimeLeftInRound
    {
        set { _timeLeftInRound += value; }
      
    }

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(this);
        }
        _manageTargets = GetComponent<TargetManager>();
        //Okay, it's not going to call spawn at start, I have to make it so it waits for user input for anchors being spawned first, or rather amount of anchors, not doing anchors right now so, first test that it works
    }
    //Instead of doing it on start, it will do when player clicks start button, just start for now
    private void Start()
    {
        //Assigning to private variable directly here, because do not want to start coroutine to spawn, before player picks amountready to spawn
        
        _timeLeftInRound = _roundTimer;
    //    GameObject dfds = Instantiate(Resources.Load("Prefabs/TargetPrefabs/orbTarget") as GameObject);
    }
    private void Update()
    {

        if (timeLeftInRound > 0)
        {
            timeLeftInRound = Time.deltaTime;
        }
        if (timeLeftInRound == _roundTimer / 2)
        {
            StartCoroutine(_manageTargets.spawnTimeTarget(10.0f, false));
        }
        if (timeLeftInRound == _roundTimer / 4)
        {
            
        }
    }
   
}
