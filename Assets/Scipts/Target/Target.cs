using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hydrogen
{
    public class Target : MonoBehaviour
    {
        //For point targets, it will add to player points, for Time targets it will add to current time left in round
        protected GameManager _gainedFromTarget;
        protected TargetManager _manageTargets;
        protected MovementManager _setMovementAndPatterns;
        protected float _penetrationThreshHold;
        //Direction the target will move, decided in algorithim at targetMovementManager
        protected Vector3 _directionMovement;
        protected int _indexPatternMovement;
        protected float maxDistance;
        protected float minDistance;
        protected float speed = 10.0f;
        protected float _amplitude;
        protected float _frequency;

        //Probably moving this into TargetMovementManager or slap somewhere else.
        protected static Dictionary<string,Pattern> _patternList = new Dictionary<string,Pattern>()
        {
            //Sin wave higher 3rd paramter, the more narrow the wave
            {"Sinwave", (float amp, float frequency, float narrowness) =>
             {
                float newPattern = amp * Mathf.Sin(Time.time * frequency);// * narrowness;
                return newPattern;
             }
            }

            //Infinity Symbol higher 3rd paramter the thicker the wave
           

        };
        char _chosenAxisForPattern;
        protected float _currentPattern;
    
        public void setPatternVars(float amp, float freq)
        {
            _amplitude = amp;
            _frequency = freq;
        }


        protected virtual void Awake()
        {
            _setMovementAndPatterns = new MovementManager();
            _manageTargets = GameObject.Find("GameManager").GetComponent<TargetManager>();
            _gainedFromTarget = GameObject.Find("GameManager").GetComponent<GameManager>();
            
        }
        protected virtual void Start()
        {
            maxDistance = GameObject.FindGameObjectWithTag("Vicinity").GetComponent<SphereCollider>().radius;
            _penetrationThreshHold = 5.0f;
            string axes = "xyz";
            _chosenAxisForPattern = axes[Random.Range(0, 2)];

            Vector3[] allDirections = new Vector3[] { transform.right, transform.up, transform.forward };
            _directionMovement = transform.right;

        //    _indexPatternMovement = Random.Range(0, _patternList.Length);
        }
        //Might change this up a bit, and prob transferring this function into TargetMovementManager.
        protected Vector3 repeatPattern(char axis)
        {
            float varX = 1.0f;
            float varY = 1.0f;
            float varZ = 1.0f;

          //  _currentPattern = _patternList[0](_amplitude, _frequency, 10.0f);

            if (axis == 'x')
            {
                varX *= _currentPattern;
                varZ *= 0;
            }
            if (axis == 'y')
            {
                varY *= _currentPattern;
                varZ *= 0;
            }
            if (axis == 'z')
            {
                varZ *= _currentPattern;
                varX *= 0;
            }
            Vector3 nextPatternMovement = new Vector3(3.0f * varX, 3.0f * varY, 3.0f * varZ);
            //It apparently goes to fucking infinity
            
            
            return nextPatternMovement;

        }

        protected virtual void Update()
        {
           
            if ((transform.position + transform.parent.position).magnitude < transform.parent.position.magnitude + maxDistance)
            {
              transform.rotation = Quaternion.identity;
            }


       //     transform.RotateAround(transform.parent.position, transform.parent.position + new Vector3(0, Mathf.Sin(Time.time * 5.0f) * 3.0f, 2.0f), 90.0f * Time.deltaTime);
       }

        protected virtual void OnTriggerEnter(Collider hit)
        {
            if (hit.gameObject.tag == "bullet")
            {
                //Adds to player points
                _gainedFromTarget.playerPoints = 5;
                //Puts it back into inactive list of targets.
                transform.parent = GameObject.Find("PooledTargets").transform;
                hit.GetComponent<Bullet>().penetration = _penetrationThreshHold;
                //Destroys the script so the object is a clean slate and can be reused as either time or normal target.
                Destroy(this);
                //Sets target active to false for reuse
                gameObject.SetActive(false);
                
            }
        }

        public void setInitial(Vector3 initialPosition, Vector3 initialMovement, Movement movementPattern)
        { }

        public void setInitial(Target copy)
        {

        }

    }
}