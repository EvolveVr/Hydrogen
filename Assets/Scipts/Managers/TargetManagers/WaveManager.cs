using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    // this is the Wave, not sure why i called it wave manager, its really the wave itself
    public class WaveManager : MonoBehaviour
    {
        //change to private laster
        public static int _currentNumberOfTarget = 0;

        private Lane[] _lanes;

        public GameObject _targetPrefab;
        private const string _targetPrefabPath = "Prefabs/TargetPrefabs/TargetAnchor";

        #region Unity Methods
        void Awake()
        {
            //_targetPrefab = Resources.Load(_targetPrefabPath) as GameObject;
            _lanes = FindObjectsOfType<Lane>();
        }

        void Start()
        {
            foreach(Lane lane in _lanes)
            {
                lane.SpawnTargetAnchor(_targetPrefab);
                _currentNumberOfTarget++;
            }
        }
        #endregion

    }
}
