using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
        
        #region Timer Variables
        private float _timeBetweenWaves = 2.0f;
        private float _currentTimeBetweenWaves = 0.0f;
        private float _timeBetweenEnemySpawns = 2.0f;
        private float _currentTimeBetweenEnemySpawns = 0.0f;
        private bool _endOfWave = false;
        #endregion

        private Lane[] _lanes;
        
        private GameObject _targetPrefab;
        private const string _targetPrefabPath = "Prefabs/TargetPrefabs/TargetAnchor";

        private Text _targetsLeftText;

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

            InitializeTargetsLeftText();
        }

        void Start()
        {
            _numberOfTargetsSpawned = 0;
            _numberOfDestroyedTargets = 0;
            currentNumberOfTarget = 0;
            _targetsLeftForWave = _numberOfTargetsPerWave;
            _totalTargetsToSpawn = _numberOfWaves * _numberOfTargetsPerWave; // this is acually false; because the number of targets varies between waves for final game

            SpawnInitialWave(); // may have to delete soom
            UpdateTargetsLeftText();
        }

        void Update()
        {
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

                SpawnTarget(_numberOfTargetsSpawned, _numberOfTargetsPerWave);
            }
            else if(_currentWave < _numberOfWaves - 1)
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
        void InitializeTargetsLeftText()
        {
            Text[] textObjects = FindObjectsOfType<Text>();

            foreach(Text text in textObjects)
            {
                if(text.name == "TargetsLeftText")
                {
                    _targetsLeftText = text;
                }
            }
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
            _targetsLeftForWave = _numberOfTargetsPerWave;      //must change later to include varied number of targets per wave
            _endOfWave = true;
        }

        private void EndOfSession()
        {
            Debug.Log("Session end");
        }
        #endregion
    }
}
