using UnityEngine;
using System.Collections;

public class moveTargets : MonoBehaviour
{
    //Attach this to targetprefab
    public delegate Vector3 moveFunction(double time, Vector3 movement);
    int _currentRound;//current round
    int _speedTarget; //speed of object
    float _waveLength;//how wide the wave is
    float _speedWave;//the time it takes to complete a wave
    float _timeSinceActive;//time since the object was set to active, keeping track so if within a time interval they will do sin pattern for example.
    Vector3 _directionMovement;//The direction the target is moving
    Vector3 _axis;//the axis the wave is happening on
    Vector3 _pos;//the current position of the target
    float _changeDirectionInterval;//Time it takes before it switches direction
    float _currentTimerBeforeChangeDirection;//Timer to keep track of time before switching
	
    void Start()
    {
        _changeDirectionInterval = 1.5f;
        _currentTimerBeforeChangeDirection = _changeDirectionInterval;
        Debug.Log(Time.time);

        _currentRound = 1;
        
        _timeSinceActive = Time.time;
        
        _directionMovement = -transform.right;
        _pos = transform.position;
        //Probably will change this on conditions met too but keeping here for now to have stable speed.
        _speedTarget = 2;
    }
	void SetMovement(moveFunction pattern)
    {
        //updates the position of the object according to pattern returned by function held by delegate
        transform.position = pattern(_timeSinceActive,_directionMovement);

    }
	
	// Update is called once per frame
	void Update ()
    { 
        if(_currentTimerBeforeChangeDirection > 0)
        {
            _currentTimerBeforeChangeDirection -= Time.deltaTime;
        }
        if(_currentTimerBeforeChangeDirection <= 0)
        {
            Debug.Log("change direction");
            //This changes the direction
            _directionMovement *= -1;
            //This resets the timer to change direction again.
            _currentTimerBeforeChangeDirection = _changeDirectionInterval;
        }
        if(_currentRound == 1)
        {
                //If below 0.8 seconds then it's first 2 targets spawned
           if (_timeSinceActive < 0.8f)
              SetMovement(roundOneSinWaves);
                //If above that point then its targets spawned after it, which is just one for now and giving you this script with that commented out since for testing you just need the cos and sin, still messing with other patterns and those definitely work.

                //Commented out since had one of statements in SinWaves function return cos, so just deleted the function it's calling for now.
    //       else if (_timeSinceActive >= 0.8f)
      //        SetMovement(roundOneCosWaves);
        }
        
	
	}
    //name currently inaccurate since have one of the functions returning cos, but in final either going to have just a patterns function or interface and patterns
    //class or keeping like this, which would be easier, but just makes it more bloated
    Vector3 roundOneSinWaves(double time, Vector3 movement)
    {
        //If keeping same the conditions will only change the length of the wave, speed and direction but will always return sin function for objects spawned
        //within that interval, I'm guessing this is what Brian meant with using the time argument passed in for handling move functions, if not let me know.
        if (time < 0.3f)
        {
           
         
            _waveLength = 3.5f;
            _speedWave = 10.0f;
            _axis = transform.up;
            _pos += _directionMovement * Time.deltaTime * _speedTarget;

        }
        else if (time > 0.3f && time < 0.6f )
        {
           
            _waveLength = 2.5f;
            _speedWave = 15.0f;
            _axis = transform.up;
            _pos += _directionMovement * Time.deltaTime * _speedTarget;
            return _pos + _axis * Mathf.Cos(Time.time * _speedWave) * _waveLength;
            //_pos += -transform.forward * Time.deltaTime * _speedTarget;
        }
        
        //returns the targets current position plus the axis the sinwave is happening in, times the width of the waves
        return _pos + _axis * Mathf.Sin(Time.time * _speedWave) * _waveLength;
    }

    
  
   

   
}
