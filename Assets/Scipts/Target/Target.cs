using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hydrogen
{
    public class Target : MonoBehaviour
    {
        //For time targets, it will add to player points, for point targets it will add to current time left in round
        protected GameManager _gainedFromTarget;
        protected TargetManager _manageTargets;
        //These variables will change depending on difficulty, except for movementDirection, that will change based on
        //distance 
        //This vector holds, _magnitude, _targetSpeed, _waveSpeed in that order.
        protected Vector4 _movementVariables;
        private float _magnitude;
        private float _targetSpeed;
        private float _waveSpeed;

        protected Vector3 _movementDirection;
        protected Vector3 _axisOfMovement;
        protected Vector3 _offSetPosition;

        protected virtual void Awake()
        {

            _manageTargets = GameObject.Find("GameManager").GetComponent<TargetManager>();
            _gainedFromTarget = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        protected virtual void Start()
        {
            _magnitude = 0.5f;
            _targetSpeed = 0.5f;
            _waveSpeed = 1.5f;
            _axisOfMovement = transform.up;
            _movementDirection = transform.right;
            _offSetPosition = transform.position;
            _movementVariables[0] = _magnitude;
            _movementVariables[1] = _targetSpeed;
            _movementVariables[2] = _waveSpeed;

            //Okay comment out any errors, otherwise it'll stop here and not do rest of lines of code after it.
            //I'm dumb, wasted so much time on this


        }

        protected void setMovement(TargetManager.moveFunction movement)
        {
            _offSetPosition += _movementDirection * Time.deltaTime * _targetSpeed;

             transform.position = movement(_movementDirection, _offSetPosition, _axisOfMovement, _magnitude, _targetSpeed, _waveSpeed);
        }
        protected void Update()
        {
            setMovement(_manageTargets.getMovement());
        }
        protected virtual void OnTriggerEnter(Collider hit)
        {
            if (hit.gameObject.tag == "Bullet")
            {
                gameObject.SetActive(false);
            }
        }


        public void setInitial(Transform initialPosition, Vector3 initialMovement, Movement movementPattern)
        {

        }

    }
}