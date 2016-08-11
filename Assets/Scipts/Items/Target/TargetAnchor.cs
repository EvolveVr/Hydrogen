using UnityEngine;
using UnityEditor.Animations;

public class TargetAnchor : MonoBehaviour
{
    private Animator _targetAnimator;
    int[] _animHashedParameters;
    string[] _animStates;

    private NavMeshAgent _navMeshAgent;
    private GameObject _navigationPlane;

    #region Unity Mehtods
    void Awake()
    {
        _targetAnimator = GetComponent<Animator>();
        _animStates = GetAllAnimatorStates(_targetAnimator);
        _animHashedParameters = GetAllHashedAnimatorParameters(_targetAnimator);
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navigationPlane = GameObject.FindGameObjectWithTag("TargetNavigation");
    }

    void Update()
    {
        if(_navMeshAgent.velocity.sqrMagnitude <= 0.001f)
        {
            _navMeshAgent.SetDestination(new Vector3(Random.Range(-20.0f, 20.0f), this.transform.position.y, this.transform.position.z));
            _targetAnimator.SetTrigger(_animHashedParameters[Random.Range(0, _animHashedParameters.Length)]);
            _targetAnimator.speed = 1 + Random.Range(-0.3f, 0.3f);
        }
    }
    #endregion

    // To randomly generate a State to transition to, I need the hash codes for each 
    // state in the state machine
    int[] GetAllHashedAnimatorParameters(Animator myAnimator)
    {
        RuntimeAnimatorController sm = myAnimator.runtimeAnimatorController;
        
        AnimatorControllerParameter[] hashedAnimatorParams = new AnimatorControllerParameter[myAnimator.parameterCount];
        int[] hashedParamNames = new int[hashedAnimatorParams.Length];
        for(int i =0; i < myAnimator.parameterCount; i++)
        {
            hashedAnimatorParams[i] = myAnimator.GetParameter(i);
            hashedParamNames[i] = hashedAnimatorParams[i].nameHash;
        }

        return hashedParamNames;
    }

    string[] GetAllAnimatorStates(Animator myAnimator)
    {
        RuntimeAnimatorController sm = myAnimator.runtimeAnimatorController;
        string[] animationStates = new string[sm.animationClips.Length];
        AnimationClip[] animStateClips = sm.animationClips;

        for (int i = 0; i < animStateClips.Length; i++)
        {
            animationStates[i] = animStateClips[i].name;
        }

        return animationStates;
    }
}
