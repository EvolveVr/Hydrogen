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
       
        #region All of the managers
        public static GameManager gameManager;
        private TargetManager _manageAnchors;
        private Anchor _manageTargets;
        #endregion

        #region Bools that check whether time target at certain time points have already been spawned

        bool halfTimeSpawnTarget;
        bool quarterTimeSpawnTarget;
        bool quarterTimeLeftSpawnTarget;

        #endregion

        #region Variables managing the round
        private Difficulty _currentDifficulty;
        private string _curentDifficulty;
        private float _roundTimer = 40.0f;
        public float _timeLeftInRound;
        private bool roundOver;
        #endregion

        private int _playerPoints;

        //  public abstract IEnumerator spawnPointTarget(int amountToSpawn);
        public string currentDifficulty
        {
            set { if (value == "Easy") _currentDifficulty = Difficulty.Easy; }
            get { return currentDifficulty; }
        }
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
            _manageAnchors = GetComponent<TargetManager>();
            _manageTargets = GetComponent<Anchor>();
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
            if (timeLeftInRound == 0)
            {
                _manageAnchors.despawnAllAnchors();       
            }
            
            if (timeLeftInRound <= _roundTimer / 2 && !halfTimeSpawnTarget)
            {
                halfTimeSpawnTarget = true;
                _manageAnchors.spawnTimeTarget();
            }
            if (timeLeftInRound <= _roundTimer / 4 && !quarterTimeSpawnTarget)
            {
                quarterTimeSpawnTarget = true;
                _manageAnchors.spawnTimeTargetAnchor();
            }
        }
    }
}