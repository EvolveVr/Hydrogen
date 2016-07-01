using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is for moving the unkillable targets while the killable targets are
/// orbiting around it.
/// </summary>
public class MoveTargets : MonoBehaviour
{   
    #region Variables for movement
    public delegate Vector3 moveFunction();//Delegate that will take in movement functions
    List<moveFunction> _movementAlgorithims;//Will store movement functions in here
    //Current index of array, this variable is how I will switch between movements
    private int _currentMovementIndex;
    //speed of object
    private float _speedTarget; 
    //how wide the wave is, magnitude
    private float _waveLength;
    //the time it takes to complete a wave
    private float _speedWave;
    //The direction and axis the target will be moving towards and on
    private Vector3 _directionMovement;
    private Vector3[] _axisMoving;
    //the axis the wave is happening on
    private Vector3 _axisOfWave;
    //the initial position of the target, used as offset
    Vector3 _initialPos;
    #endregion

    #region Variables for checking conditions to move
    public float _changeMovementInterval = 6.0f;
    public float _timeLeftToChangeMovement;
    public float _changeDirectionInterval = 4.0f;
    public float _timeLeftToChangeDirection;
    public float _changeMovementAxisInterval = 20.0f;
    public float _timeLeftToChangeAxis;
    #endregion
    
 
    //This instantiates the the arrays
    private void Awake()
    {

       
        _axisMoving = new Vector3[3];
        _movementAlgorithims = new List<moveFunction>();
    }
 
    //This initializes the list of movement Functions
    private void initializeMovementAlogirithms()
    {
        //Sin wave function
       _movementAlgorithims.Add( () =>
           {

               //Might need to add conditions for both length and wave too, honestly.
               if (_directionMovement == transform.right)
                   _axisOfWave = transform.up;
               else if (_directionMovement == transform.up)
                   _axisOfWave = transform.right;
               _waveLength = 2.5f;
               

               return _initialPos + _axisOfWave * Mathf.Sin(Time.time * _speedWave) * _waveLength;
           });
        //Cosine wave function
        _movementAlgorithims.Add( () =>
        {

            _waveLength = 3.5f;
            if (_directionMovement == transform.right)
                _axisOfWave = transform.up;
            else if (_directionMovement == transform.up)
                _axisOfWave = transform.right;

            return _initialPos + _axisOfWave * Mathf.Cos(Time.time * _speedWave) * _waveLength;
        });
        //Spiral function
        _movementAlgorithims.Add( () =>
        {
            _waveLength = 5.0f;
            if (_directionMovement == transform.right || _directionMovement == -transform.right)
                _axisOfWave = transform.up;
            else if (_directionMovement == transform.up || _directionMovement == -transform.up)
                _axisOfWave = transform.right;
            Vector3 spiral = new Vector3();
            spiral.x = Mathf.Cos(Time.time * _speedWave) * _waveLength;
            spiral.y = Mathf.Sin(Time.time * _speedWave) * _waveLength; 
            //Can barely tell it's spiraling forward in back, visually, unless look up, butprob just better to leave like this,for now
            //spiral.z = Mathf.Sin(Time.time * _speedWave) * _waveLength;
            return _initialPos + spiral;

        });

    }
    
    private void Start()
    {
        #region Timers being set
        _timeLeftToChangeMovement = _changeMovementInterval;
        _timeLeftToChangeDirection = _changeDirectionInterval;
        _timeLeftToChangeAxis = _changeMovementAxisInterval;
        #endregion

        #region Setting speed variables
        _speedTarget = 2.0f;
        _speedWave = 2.0f;
        #endregion

        #region Setting the variables to be ready for movement
        _axisMoving[0] = transform.right;
        _axisMoving[1] = transform.up;
        initializeMovementAlogirithms();
        _initialPos = transform.position;

        _currentMovementIndex = Random.Range(0, _movementAlgorithims.Count);
        _directionMovement = _axisMoving[Random.Range(0, 1)];
        #endregion

    }
    
    /// <summary>
    /// This function decreases all of the timers
    /// and executes what they need to depending on total timer interval
    /// differentiating what the timer is for.
    /// </summary>
    /// <sidenote>
    /// Similar function is in targetmanager, I could make that public and call it instead of remaking in here
    /// but since this one is dependant on variables within class, I would need to make hooks to get them from targetManager
    /// for no other reason then for the timer, which seems pretty pointless, even if it gets rid of duplicate code
    /// it will be replaced with same amount of lines of code
    /// </sidenote>
    /// <param name="currentTime">The timer that will be decrementing</param>
    /// <param name="maxTime">The total time it starts from and will reset to</param>
    private void decreaseTime(ref float timeLeft,float maxTime)
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        if(timeLeft <= 0)
        {
            if (maxTime == _changeDirectionInterval)
                _directionMovement *= -1;
            else if (maxTime == _changeMovementInterval)
                _currentMovementIndex = Random.Range(0, _movementAlgorithims.Count);
            else if (maxTime == _changeMovementAxisInterval)
                _directionMovement = _axisMoving[Random.Range(0, 1)];
            timeLeft = maxTime;            
        }
    }
    

    private void Update()
    {
        decreaseTime(ref _timeLeftToChangeDirection, _changeDirectionInterval);
       // decreaseTime(ref _timeLeftToChangeMovement, _changeMovementInterval);
        decreaseTime(ref _timeLeftToChangeAxis, _changeMovementAxisInterval);

        setMovement(_movementAlgorithims[_currentMovementIndex]);
    }

    #region setMovement function and its description.
    /// <summary>
    /// This function updates the offset.
    /// And updates current position with vector3 returned by delegate.
    /// </summary>
    /// <param name="pattern">The parameter is a delegate that will take in lambdas stored in _movementAlgorithims</param>
    private void setMovement(moveFunction pattern)
    {
        //Updates initial position with direction currently going, the offset
        _initialPos += _directionMovement * Time.deltaTime * _speedTarget;

        //Updates current position
        transform.position = pattern();

    }
    #endregion

    //Checks to see if bullet hit it, if did: destroy bullet
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "bullet")
            Destroy(hit.gameObject);
    }
}
