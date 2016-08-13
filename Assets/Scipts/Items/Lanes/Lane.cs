using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// Only one target can Occupy a lane. If a target is destroyed
    /// then a lane is no longer occupied.
    /// </summary>
    public class Lane : MonoBehaviour
    {
        private Transform[] _pointsInSpace;

        private bool _isActive = false;

        #region Properties
        public Transform[] PointsInSpace { get { return _pointsInSpace; } }

        public int NumberOfPoints { get { return _pointsInSpace.Length; } }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        // need to return a point from a lane
        public Transform this[int index]
        {
            get { return _pointsInSpace[index]; }
        }
        #endregion

        #region Unity Mehtods
        void Awake()
        {
            _pointsInSpace = GetComponentsInChildren<Transform>();
        }
        #endregion

        #region Methods
        // Spawn prefab, set it at one of the endpoints, and set TargetAcnhors Lane and set isPccupioed to true
        public bool SpawnTargetAnchor(GameObject targetPrefab)
        {
            GameObject target = Instantiate(targetPrefab, _pointsInSpace[0].position, Quaternion.identity) as GameObject;
            TargetAnchor targetAnchor = target.GetComponent<TargetAnchor>();
            
            if(targetAnchor != null)
            {
                targetAnchor.SetLane(this);
                _isActive = true;
                return true;
            }
            else
            {
                Debug.LogError("Target anchor was not found");
            }

            return false;
        }

        #endregion

    }
}
