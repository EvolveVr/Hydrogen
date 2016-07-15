using UnityEngine;
using System.Collections;
/// <summary>
/// This script is for orbiting the killable target around the unkillable one
/// Will need to change script names a bit, it's descriptive in purpose but misleading with the unkillable target having move target, it's
/// technically not a target, since don't want to hit it.
/// </summary>
public class OrbitTarget : MonoBehaviour
{
    #region Variables handling how orbit will act
    private Transform _targetOfOrbit;
    private Vector3 _axisOfOrbit;
    private float _timeSwitchAxis = 5.0f;
    private float _currentTimeLeftToSwitch;
    private float _speedOfOrbit;
    #endregion

    private void Awake ()
    {
       // Debug.Log(transform.parent.name);
        //Get componenet in parent for whatever reason wasn't working but this does so doing it like this, it does same thing anyway.
        _targetOfOrbit = transform.parent.transform;
        _axisOfOrbit = new Vector3();
	}

    private void Start()
    {
        //Setting to 0 first so I can initialize axis to correct position at start.
        _currentTimeLeftToSwitch = 0;
        int initialAxis = Random.Range(0, 100);
        //If there is a remainder then it's not even, but if it is even then initial axis of orbis will be x axis
        if (initialAxis % 2 == 0)
        {
            _axisOfOrbit = _targetOfOrbit.right;
        }
        else
            _axisOfOrbit = _targetOfOrbit.up;
            _speedOfOrbit = 45.0f;
    }
	private void Update ()
    {
        if (_currentTimeLeftToSwitch > 0)
        {
            _currentTimeLeftToSwitch -= Time.deltaTime;
        }
        if (_currentTimeLeftToSwitch <= 0)
        {
            if (_axisOfOrbit == _targetOfOrbit.right)
                _axisOfOrbit = _targetOfOrbit.up;
            else if (_axisOfOrbit == _targetOfOrbit.up)
                _axisOfOrbit = _targetOfOrbit.right;

            _currentTimeLeftToSwitch = _timeSwitchAxis;
        }

        transform.RotateAround(_targetOfOrbit.position, _axisOfOrbit + new Vector3(0, Mathf.Sin(Time.time * 5.0f)), Time.deltaTime * _speedOfOrbit);
	
	}
}
