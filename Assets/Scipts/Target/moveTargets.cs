using System.Collections.Generic;
using UnityEngine;
public class moveTargets : MonoBehaviour
{


    //Temporary to make hitting targets easier
    private float _changeInterval = 0.5f;
    private float _timeBeforeChange;
    /*Variables for checking conditions to move*/
  
    private Dictionary<string, Vector3> _angledRays;
    
    //These are values used to test where bullet landed
    private float _close;
    private float _kindaClose;
    private float _playerBlind;

    //Variables for movement algorithims
    public delegate Vector3 moveFunction();//Delegate that will take in movement functions
    Dictionary<string, moveFunction> _movementAlgorithims;//Will store movement functions in here
    float _speedTarget; //speed of object
    float _waveLength;//how wide the wave is
    float _speedWave;//the time it takes to complete a wave
    Vector3 _directionMovement;//The direction the target is moving
    string _nextMovement;//This will be assigned keys to dictionary depending on current position of target and get the next move algorithm
    Vector3 _axis;//the axis the wave is happening on
    Vector3 _pos;//the current position of the target
    
    //Raycasting will make this sooo much easier in tracking position relative to other colliders, better than manual method had before with loop
    //and trying to be precise with increments
    
     
    void Awake()
    {
        _angledRays = new Dictionary<string, Vector3> ();
        _movementAlgorithims = new Dictionary<string, moveFunction> ();
    }
    //To avoid clogging the start function, should probably seperate this 
    void initializeAnglesOfRays()
    {
        //will store vector 3s in _angledRays

    }
    //Adds in lambdas that return move algorithm to list
    void initializeMovementAlogirithms()
    {
        _movementAlgorithims["MoveDown"] = () =>
        {
            //_directionMovement = -transform.up;
            _waveLength = 6.5f;
            _speedWave = 3.0f;
            _axis = transform.forward;

            return _pos + _axis * Mathf.Cos(Time.time * _speedWave) * _waveLength;

        };
        _movementAlgorithims["MoveLeft"] = () =>
        {
            //The current scene has z axis for left and right
           // _directionMovement = -transform.forward;
            _waveLength = 8.5f;
            _speedWave = 4.0f;
            _axis = transform.up;
            return _pos + _axis * Mathf.Sin(Time.time * _speedWave) * _waveLength;
        };
        _movementAlgorithims["MoveRight"] = () =>
        {
            //_directionMovement = transform.forward;
            _waveLength = 3.5f;
            _speedWave = 2.0f;
            _axis = transform.up;
            return _pos + _axis * Mathf.Cos(Time.time * _speedWave) * _waveLength;
        };
        _movementAlgorithims["MoveUp"] = () =>
        {
           // _directionMovement = transform.up + transform.right;
            _waveLength = 3.5f;
            _speedWave = 2.0f;
            _axis = transform.up;
            return _pos + _axis * Mathf.Sin(Time.time * _speedWave) * _waveLength;
        };

    }

    void Start()
    {
        _timeBeforeChange = _changeInterval;
        _close = 3.0f;
        _kindaClose = 5.0f;
        _playerBlind = 7.0f;

        _speedTarget = 0.5f;
        initializeMovementAlogirithms();
        _pos = transform.position;

        //This checks for what time target was spawned to assign initial movement
        if (Time.time < 0.4f)
        {
            _nextMovement = "MoveLeft";
            _directionMovement = transform.forward;
        }
        else if (Time.time > 0.4f && Time.time < 0.8f)
        {
            _directionMovement = transform.right;
            _nextMovement = "MoveRight";
        }
        else
        {
            _directionMovement = transform.up;
            _nextMovement = "MoveDown";
        }
    }
    //Function the lambdas are passed to
	void SetMovement(moveFunction pattern)
    {
       
        //Updates initial position with direction currently going
        _pos += _directionMovement * Time.deltaTime * _speedTarget;

        //Updates current position
        transform.position = pattern();
    }
	
	void checkTargetPosition(double distance, string direction)
    {
     
        if (distance <= _close && distance > 0)
        {
            switch (direction)
            {
                //The next movements will change, this is just layout of how I'll handle updating move algorithim once get into actual level
                //It will depend on distance between colliders and current direction of the target, just used same to test and it works
                case "right":
                    _nextMovement = "MoveRight";
                    break;
                case "left":
                    _nextMovement = "MoveLeft";
                    break;
                case "down":
                    _nextMovement = "MoveDown";
                    break;

            }
        }         
        else if (distance > _close && distance < _kindaClose)
        { }
        else if (distance > _kindaClose && distance < _playerBlind)
        { }

    }
	
    void FixedUpdate()
    {

       

        RaycastHit distanceFromWalls = new RaycastHit();
        //Right now just calling same as the trigger enter thing, mainly cause don't have exact distances for what level itself will be like
        //to compare, and haven't decided movement patterns cause going to make them based off level so it doesn't clip through shit and can use
        //obstacles in there to take advantage of with obstacles.
        /*Commented out cause testing speeds for simple movements first*/
        if (Physics.Raycast(transform.position, transform.up, out distanceFromWalls))
        {
            checkTargetPosition(distanceFromWalls.distance, "down");        
        }
        else if (Physics.Raycast(transform.position,-transform.right, out distanceFromWalls))
        {
            checkTargetPosition(distanceFromWalls.distance, "right");
        }
        if (Physics.Raycast(transform.position, transform.right+ transform.up, out distanceFromWalls))
        {
           
            if (distanceFromWalls.transform.gameObject.tag == "right" && (distanceFromWalls.distance >= 8.0f && distanceFromWalls.distance < 9.0f))
            {
                _nextMovement = "MoveLeft";
            }
        }
        //This updates the targets next movement
        if (_nextMovement != null)
        {
          
            SetMovement(_movementAlgorithims[_nextMovement]);

        }
        if( _timeBeforeChange > 0)
        {
            _timeBeforeChange -= Time.deltaTime;
        }
        if (_timeBeforeChange <= 0)
        {
            _directionMovement *= -1;
            _timeBeforeChange = _changeInterval;
        }
            
        
    }
    void OnTriggerEnter(Collider hit)
    {
        Debug.Log(hit.gameObject.name);
        switch(hit.gameObject.tag)
        {
           
            //This works, honestly could delete this, and just check this for when raycast distance is == 0
            case "left":
                //If wall is to left target will start moving in opposite direction
                _nextMovement = "MoveRight";
                break;
            case "right":
                //viceversa
                _nextMovement = "MoveLeft";
                break;
            case "ceiling":
                //If target is nearing the ceiling, start doing a pattern downwards, or whatever, this is just template
                _nextMovement = "MoveDown";
                break;
                //If target getting too low;
            case "bottom":
                _nextMovement = "MoveUp";
                break;
        }
    }
   
}
