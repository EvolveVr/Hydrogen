using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Hydrogen
{
    // this is the Wave, not sure why i called it wave manager, its really the wave itself
    public class WaveManager : MonoBehaviour
    {
        // change to private later
        private static int currentNumberOfTarget = 0;
        private int _numberOfDestroyedTargets = 0;
        public int totalTargetsToSpawn = 5;                //change private later
        private int _numberOfTargetsSpawned = 0;

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
            get { return totalTargetsToSpawn; }
            set
            {
                // May have to add logic here to make it more balance, review later!!!!!!!!!!!!!!
                totalTargetsToSpawn = value;
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

            SpawnInitialWave();
            UpdateTargetsLeftText();
        }

        void Update()
        {
            
            if(_numberOfTargetsSpawned < totalTargetsToSpawn)
            {
                int inactiveLaneIndex = GetInactiveLane(_lanes);
                if (inactiveLaneIndex != -1)
                {
                    _lanes[inactiveLaneIndex].SpawnTargetAnchor(_targetPrefab);
                    _numberOfTargetsSpawned++;
                }
            }
            else if(_numberOfDestroyedTargets == totalTargetsToSpawn)
            {
                Debug.Log("game bruh");
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
            _targetsLeftText.text = string.Format("Targets Left for Wave: {0}", totalTargetsToSpawn - _numberOfDestroyedTargets);
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
        #endregion
    }
}
