using UnityEngine;
using System.Collections;
/// <summary>
/// This script handles amount of enemies to spawn depending on currentWave which is updated
/// by TargetManager, might switch that around since think should actually be other way around
/// </summary>
public class GameManager : MonoBehaviour {
    public static GameManager gameManager;
    //This variable will be used to set amount to spawn and call the spawnTarget function
    private TargetManager _manageTargets;

    private int _playerPoints;
    private int _currentWave;
    
    //This function changes spawn count depending on wave
    private void changeSpawnCount()
    {
        switch (_currentWave)
        {
            //Set to 0 so only spawns wave 1 for now, was to test orbiting
            case 2:
                _manageTargets.amountToSpawn = 1;
                break;
            case 3:
                _manageTargets.amountToSpawn = 1;
                break;
            default:
                _manageTargets.amountToSpawn = 0;
                break;
        }
    }

    /// <summary>
    /// This adds to player points and returns the tallied up score when round is over.
    /// </summary>
    public int playerPoints
    {
        set { _playerPoints += value; }
        get { return _playerPoints; }//Returning for UI
    }

    /// <summary>
    /// This gets updates to current wave and calls the
    /// changeSpawnCount() function to set amount to spawn then calls
    /// spawnTarget() function to spawn the targets
    /// </summary>
    public int currentWave
    {
        set
        {
            _currentWave += value;
            changeSpawnCount();
            StartCoroutine(_manageTargets.spawnTarget());
        }
    }

    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(this);
        }
        _manageTargets = gameObject.GetComponent<TargetManager>();
    }
 
    private void Start()
    {
        //waves in this case, isn't end of round, just next wave of enemies spawned in time intervals
        _currentWave = 1;
        _manageTargets.amountToSpawn = 1;
        StartCoroutine(_manageTargets.spawnTarget());
    }
    


}
