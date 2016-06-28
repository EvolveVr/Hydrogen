using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    public class ChargeGun : Gun
    {
        //A gun that charges needs 1) max charge time, 2) and the current charge time
        public float maxChargeTime = 5.0f;
        private float _chargeTime = 0f;

        protected override void Update()
        {
            base.Update();


        }

        public override void UseButtonPressed()
        {
            _chargeTime += Time.deltaTime;

            if (_chargeTime >= maxChargeTime)
            {
                _chargeTime = maxChargeTime;
            }
        }

        public override void UseButtonUp()
        {
            //only shoot when it is charged enough
            if (_chargeTime >= maxChargeTime)
            {
                shootGun();
            }

            _chargeTime = 0f;
        }
    } // end of Charge class

}
