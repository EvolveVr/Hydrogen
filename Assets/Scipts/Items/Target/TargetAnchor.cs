using UnityEngine;
using UnityEditor.Animations;

namespace Hydrogen
{
    /// <summary>
    /// This class is used to control target Movement in Space
    /// </summary>
    public class TargetAnchor : MonoBehaviour
    {
        private Animator _targetAnimator;
        int[] _animHashedParameters;

        private NavMeshAgent _navMeshAgent;
        public float navMeshSpeed = 2.0f;
        private GameObject _navigationPlane;

        private Lane _myLane;
        private int _nextPoint = 0;

        #region Properties
        public Lane GetLane { get { return _myLane; } }
        #endregion

        #region Unity Mehtods
        void Awake()
        {
            _targetAnimator = GetComponent<Animator>();
            _animHashedParameters = GetAllHashedAnimatorParameters(_targetAnimator);
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navigationPlane = GameObject.FindGameObjectWithTag("TargetNavigation");
        }

        void Update()
        {
            if ( (_navMeshAgent.velocity.sqrMagnitude <= 0.001f  && _navMeshAgent.remainingDistance <= 0.01f) && (_myLane != null) )
            {
                //Currently, just picking random points in space, but now need to change this to picking specific points in the Lane
                Transform newPoint = GetNextPoint(_myLane, ref _nextPoint);

                _navMeshAgent.SetDestination(new Vector3(newPoint.position.x, newPoint.position.y, newPoint.position.z));
                _navMeshAgent.speed = navMeshSpeed;

                _targetAnimator.SetTrigger(_animHashedParameters[Random.Range(0, _animHashedParameters.Length)]);
                _targetAnimator.speed = 1 + Random.Range(0, 1.0f);
            }
            
        }
        #endregion

        #region Methods
        // To randomly generate a State to transition to, I need the hash codes for each 
        // state in the state machine
        int[] GetAllHashedAnimatorParameters(Animator myAnimator)
        {
            AnimatorControllerParameter[] hashedAnimatorParams = new AnimatorControllerParameter[myAnimator.parameterCount];
            int[] hashedParamNames = new int[hashedAnimatorParams.Length];
            for (int i = 0; i < myAnimator.parameterCount; i++)
            {
                hashedAnimatorParams[i] = myAnimator.GetParameter(i);
                hashedParamNames[i] = hashedAnimatorParams[i].nameHash;
            }

            return hashedParamNames;
        }

        public void SetLane(Lane lane)
        {
            if(_myLane == null)
                _myLane = lane;
        }

        private Transform GetNextPoint(Lane myLane, ref int nextPoint)
        {
            // eventually, it will go out of bounds without modulus of length of lane
            nextPoint = (nextPoint + 1) % (_myLane.NumberOfPoints);
            return myLane[nextPoint];
        }
        #endregion

    }
}
