﻿using UnityEngine;
using UnityEngine.UI;
using System;

namespace Hydrogen
{
    // this is the Wave, not sure why i called it wave manager, its really the Wave itself
    public class WaveManager : MonoBehaviour
    {
        // change to private later
        private static int currentNumberOfTarget = 0;
        private int _numberOfDestroyedTargets = 0;
        private int _totalTargetsToSpawn;
        public int _numberOfTargetsPerWave = 5;
        private int _numberOfTargetsSpawned = 0;
        private int _numberOfWaves = 3;             // this will be changed by proceeding wavemanager
        private int _currentWave = 0;
        private int _targetsLeftForWave;
        private int[] _numberOfTargetsForEachRound;
        
        #region Timer Variables
        private float _timeBetweenWaves = 3.0f;
        private float _currentTimeBetweenWaves = 0.0f;
        private float _timeBetweenEnemySpawns = 0.4f;
        private float _currentTimeBetweenEnemySpawns = 0.0f;
        private bool _endOfWave = false;
        public float _maxTimeForSession = 40.0f;       //in seconds right now
        private float _currentSessionTime = 0.0f;
        #endregion

        private Lane[] _lanes;
        private GameManager _gameManager;
        
        private GameObject _targetPrefab;
        private const string _targetPrefabPath = "Prefabs/TargetPrefabs/TargetAnchor";

        private Text _targetsLeftText;
        private Text _timerText;
        private Button _nextSessionButton;

        #region Properties
        public static int CurrentNumberOfTarget
        {
            get { return currentNumberOfTarget; }
            set { currentNumberOfTarget = value; }
        }

        public int TotalTargetsToSpawn
        {
            get { return _totalTargetsToSpawn; }
            set
            {
                // May have to add logic here to make it more balance, review later!!!!!!!!!!!!!!
                _totalTargetsToSpawn = value;
            }
        }

        public int TargetsLeftForWave
        {
            get { return _targetsLeftForWave; }
            set
            {
                if (value == (_targetsLeftForWave - 1))
                    _targetsLeftForWave = value;
            }
        }
        #endregion

        #region Unity Methods
        void Awake()
        {
            _targetPrefab = Resources.Load(_targetPrefabPath) as GameObject;
            _lanes = FindObjectsOfType<Lane>();
            Utility.InitGameObjectComponent("GameManager", out _gameManager);
            Utility.InitGameObjectComponent("TimerText", out _timerText, byName:true);

            InitializePointCanvasElements();
        }

        void Start()
        {
            InitNextSessionValues();
            SpawnInitialWave(); // may have to delete soon
            UpdateTargetsLeftText();
        }

        // basic game logic
        void Update()
        {
            // if there time runs out
            _currentSessionTime += Time.deltaTime;
            UpdateTimerText(_currentSessionTime);
            if (_currentSessionTime >= _maxTimeForSession)
            {
                //EndOfSession();
                _gameManager.EndOfGame(this);
            }

            // Finish the Wave
            if (_targetsLeftForWave > 0)
            {
                //wait to spawn new Targets if it is the end of the round
                if (_endOfWave)
                {
                    _currentTimeBetweenWaves += Time.deltaTime;
                    if(_currentTimeBetweenWaves >= _timeBetweenWaves) {
                        _endOfWave = false;
                        _currentTimeBetweenWaves = 0.0f;
                    }
                    return;
                }

                //NOTE!!!
                SpawnTarget(_numberOfTargetsSpawned, _numberOfTargetsForEachRound[_currentWave]);
            }
            // when the Wave is complete
            else if(_currentWave < _numberOfTargetsForEachRound.Length - 1)
            {
                EndOfWave();
            }
            else
            {
                EndOfSession();
            }
        }
        #endregion

        #region Methods
        public void InitNextSessionValues()
        {
            //DetermineNumberOfTargets(ref _gameManager.lastSessionTotalTargets, ref _gameManager.lastSessionTotalRounds, out _numberOfTargetsForEachRound);
            DetermineNumberOfTargets(_gameManager.TotalNumberOfTargets, _gameManager.LastSessionTotalRounds, out _numberOfTargetsForEachRound);
            _numberOfTargetsSpawned = 0;
            _numberOfDestroyedTargets = 0;
            currentNumberOfTarget = 0;
            _currentWave = 0;
            _targetsLeftForWave = _numberOfTargetsForEachRound[_currentWave];

            // need to access GameManager for knowing what the previous state of the Game Was;
            // The GameManager will know the Previous State such as how many targets there were in the last wave of the match
            // for now, this will do
            _totalTargetsToSpawn = _gameManager.TotalNumberOfTargets;
            UpdateTargetsLeftText();
        }

        void InitializePointCanvasElements()
        {
            Text[] textObjects = FindObjectsOfType<Text>();
            foreach(Text text in textObjects)
            {
                if(text.name == "TargetsLeftText")
                {
                    _targetsLeftText = text;
                }
            }

            Utility.InitGameObjectComponent("NextSessionButton", out _nextSessionButton, byName:true);
        }

        // Method returning an inactive lane
        int GetInactiveLane(Lane[] myLanes)
        {
            for(int i = 0; i < myLanes.Length; i++)
            {
                if(myLanes[i].IsActive == false)
                {
                    return i;
                }
            }

            return -1;
        }

        public void IncrementNumberOfDestroyedTargets()
        {
            _numberOfDestroyedTargets++;
        }

        public void UpdateTargetsLeftText()
        {
            _targetsLeftText.text = string.Format("Targets Left for Session: {0}", _totalTargetsToSpawn - _numberOfDestroyedTargets);
        }

        public void UpdateTimerText(float time)
        {
            _timerText.text = Math.Round(_maxTimeForSession - time, 1).ToString();
        }

        private void SpawnInitialWave()
        {
            // spawn target in each lane off start, change later
            foreach (Lane lane in _lanes)
            {
                lane.SpawnTargetAnchor(_targetPrefab);
                currentNumberOfTarget++;
                _numberOfTargetsSpawned++;
            }
        }

        // basic logic for ONE wave; dont eneter if no time
        private void SpawnTarget(int targetsSpawned, int totalTargetsForWave)
        {
            // only spawn a target if number of targets already spawned is less than number of targets we need to spawn
            if (targetsSpawned < totalTargetsForWave && _currentTimeBetweenEnemySpawns >= _timeBetweenEnemySpawns)
            {
                int inactiveLaneIndex = GetInactiveLane(_lanes);
                if (inactiveLaneIndex != -1)
                {
                    _lanes[inactiveLaneIndex].SpawnTargetAnchor(_targetPrefab);
                    _numberOfTargetsSpawned++;
                }
                _currentTimeBetweenEnemySpawns = 0.0f;
            }
            else
            {
                _currentTimeBetweenEnemySpawns += Time.deltaTime;
            }
        }

        private void EndOfWave()
        {
            currentNumberOfTarget = 0;
            _currentWave++;
            _numberOfTargetsSpawned = 0;
            _targetsLeftForWave = _numberOfTargetsForEachRound[_currentWave];
            _endOfWave = true;
        }

        private void EndOfSession()
        {
            //kill all targets if there are some
            DestroyAllTargetsOnField();

            Debug.Log("Session end, Time has NOT ran out");
            _gameManager.NumberOfSessionsPlayed += 1;

            // will need to change this to enable the trigger collider on GameObject
            // enable animations aas well
            _nextSessionButton.gameObject.SetActive(true);
            _nextSessionButton.interactable = true;

            // Give the  GameManager the Current state of the game so that this info is not lost
            // for the next session; Add this Later

            enabled = false;
        }

        public void DestroyAllTargetsOnField()
        {
            TargetAnchor[] targets = FindObjectsOfType<TargetAnchor>();

            foreach(TargetAnchor target in targets)
            {
                Destroy(target.gameObject);
            }
        }

        //Algorithm for determinging targets for each wave
        public void DetermineNumberOfTargets(ref int previousTargetCount, ref int previousNumberOfRounds, out int[] eachRoundTargetNumber)
        {
            previousNumberOfRounds += GameManager.IncreaseWaveCount;
            eachRoundTargetNumber = new int[previousNumberOfRounds];
            int totalTargetsForNextSession = previousTargetCount;

            for(int i = 0; i < eachRoundTargetNumber.Length; i++)
            {
                totalTargetsForNextSession += GameManager.IncreaseTargetCount;
                eachRoundTargetNumber[i] = totalTargetsForNextSession;
            }
            Debug.Log(totalTargetsForNextSession);
            previousTargetCount = totalTargetsForNextSession;
        }

        public void DetermineNumberOfTargets(int previousTargetCount, int previousNumberOfRounds, out int[] eachRoundTargetNumber)
        {
            int currentSessionRoundNumber = previousNumberOfRounds + GameManager.IncreaseWaveCount;
            _gameManager.LastSessionTotalRounds = currentSessionRoundNumber;

            eachRoundTargetNumber = new int[currentSessionRoundNumber];
            
            int totalTargetsForRound = previousTargetCount;
            for (int i = 0; i < eachRoundTargetNumber.Length; i++)
            {
                totalTargetsForRound = totalTargetsForRound + GameManager.IncreaseTargetCount;
                eachRoundTargetNumber[i] = totalTargetsForRound;
            }

            int totalTargets = 0;
            foreach (int num in eachRoundTargetNumber)
            {
                totalTargets += num;
            }

            _gameManager.lastSessionTotalTargets = totalTargets;
        }
        #endregion
    }
}
