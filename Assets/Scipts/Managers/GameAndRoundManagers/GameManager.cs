using UnityEngine;
using System.Collections;
/// <summary>
/// This script handles amount of enemies to spawn depending on currentWave which is updated
/// by TargetManager, might switch that around since think should actually be other way around
/// </summary>
namespace Hydrogen
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager;
        private TargetManager _manageTargets;
       
        #region These variables will be bools to see if it's spawned the timeTarget at specfic time, that way it won't continously do it. It'll still check and checks are expensive, but fuck.

        bool halfTimeSpawnTarget;
        bool quarterTimeSpawnTarget;
        bool quarterTimeLeftSpawnTarget;
        
        #endregion
            
        protected Vector3 spawnPosition;
        protected int _maxSpawnAtATime;

        #region Variables managing the round
        private float _roundTimer = 30.0f;
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
        }
        //Instead of doing it on start, it will do when player clicks start button, just start for now
        private void Start()
        {
     
  
            _timeLeftInRound = _roundTimer;
        
        }
        private void Update()
        {

            if (timeLeftInRound > 0)
            {
                timeLeftInRound = Time.deltaTime;
            }
            if (timeLeftInRound <= _roundTimer / 2 && !halfTimeSpawnTarget)
            {
                halfTimeSpawnTarget = true;
                _manageTargets.callSpawner("time");
            }
            if (timeLeftInRound <= _roundTimer / 4 && !quarterTimeSpawnTarget)
            {
                quarterTimeSpawnTarget = true;
                _manageTargets.callSpawner("time", 0, true);
            }
       
        }

    }
}