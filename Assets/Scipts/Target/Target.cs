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
        protected MovementManager _setMovement;
        protected float _penetrationThreshHold;
        protected Vector3 _directionMovement;
        int speed = 2;
        float step;
        protected static int[] sfsdf = { 34, 34, 34 };
        protected static Movement[] targetMovementList = new Movement[] {

      /*      (Vector3 d, Vector3 a, float m, float ws) =>
            {
                Vector3 movement = new Vector3();
                movement.y = Mathf.Sin(Time.time * ws) * m;
             return movement;
            },

            (Vector3 a,Vector3 b, float m ,float ws) =>
            {
                Vector3 movement = new Vector3();
                movement.y = m * Mathf.Sin(Time.time * ws) + m;
                return a + b + movement;

            }


    */
        };
            
        protected Vector3 _initialPosition;


        //I can still use my method but just slapping it into anchor so setting spawn point is same time actually spawning it.
        //Pattern will be equation itself like pass in resulting number of sin wave.
        public void setInitial(Vector3 initPosition, Vector3 initDirection, float pattern)
        {


        }

        protected virtual void Awake()
        {
            _setMovement = new MovementManager();
            _manageTargets = GameObject.Find("GameManager").GetComponent<TargetManager>();
            _gainedFromTarget = GameObject.Find("GameManager").GetComponent<GameManager>();
            
        }
        protected virtual void Start()
        {
            _penetrationThreshHold = 5.0f;
           
        }
        
      
        protected virtual void Update()
        {
           // _initialPosition += _directionMovement * Time.deltaTime * speed;
            transform.Translate(Vector3.right * speed * Time.deltaTime);
         //   transform.position = targetMovementList[0](_initialPosition, transform.u, 0.5f, 2.5f);
        }

        protected virtual void OnTriggerEnter(Collider hit)
        {
            if (hit.gameObject.tag == "Bullet")
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
        protected void OnTriggerExit(Collider other)
        {
            if (other.tag == "Vicinity")
            {
                int speed = 4;
                _directionMovement = _setMovement.generateTargetMovement(0.2f, 0.7f, speed, 45.0f)(transform, other.transform, _directionMovement);
            
                //When it hits viciinity it wont just bounce back, but it will instead curve in opposite direction
                //So this will call function inside movementmanager to generate target movement based on last movement, so I know it hit vicinity because of trigger
                //and I'll know from movement passed in, what direction it was coming from, my way did this but  needed if statements and I'm all for alternatives.
            }
        }
    }
}