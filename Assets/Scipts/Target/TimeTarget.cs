using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    public class TimeTarget : Target
    {
        private float timeTillLeave = 20.0f;
        private Renderer rend;
        protected override void Awake()
        {
            base.Awake();
            rend = gameObject.GetComponent<Renderer>();
            
        }
        protected override void Start()
        {
            base.Start();
            //Changes color of object so that players can tell it's a time target
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.cyan);
            _penetrationThreshHold = 30.0f;
        }

        protected override void Update()
        {
            base.Update();

            timeTillLeave -= Time.deltaTime;
            //If time to despawn hits 0 or below, remove this script form this object, and send time targets back to pooled targets and anchor just inactive
            //and all of their colors will be reset to standard.
            if (timeTillLeave <= 0)
            {
                Destroy(this);
                if (gameObject.name != "Anchor(Clone)")
                    transform.parent = GameObject.Find("PooledTargets").transform;
                    //Reverts back to normal target color, so we can reuse this object as a point target or normal anchor
                rend.material.shader = Shader.Find("Standard");
                
                gameObject.SetActive(false);
            }
        }
        protected override void OnTriggerEnter(Collider hit)
        {
            base.OnTriggerEnter(hit);
            //For time targets that have anchor as target, they will get tagged with obstacle.
            if (hit.gameObject.tag == "Bullet")
            {
                _gainedFromTarget.addTimeLeftInRound = 5.0f;
            }
           
        }
    }
}