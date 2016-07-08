using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    public class PointTarget : Target
    {

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnTriggerEnter(Collider hit)
        {

            if (hit.gameObject.tag == "Bullet")
            {

                //Spawns a point target to replace one lost, and checks to see if need to replace more to get amountSpawnedAtATime
                //I hate constantly checking but since coroutine not working for whatever reason.
             //   _manageTargets.callSpawner("point", 1);
                _gainedFromTarget.playerPoints = 5;
                //Okay I was worried that destroying this script would auto cut it off and not execute next line of code which is base, but it did so WOO.
                Destroy(this);
                base.OnTriggerEnter(hit);
            }
            //Actually since disabling this script then it won't call this first.
            //Destroy(this);

        }
    }
}