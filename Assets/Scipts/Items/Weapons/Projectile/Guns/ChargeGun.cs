using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// This is a basic class for Guns that Charge.
    /// </summary>
    public class ChargeGun : Gun
    {
        //A gun that charges needs 1) max charge time, 2) and the current charge time
        [Header("Fields below from ChargeGun class")]
        [Space(15)]
        public float maxChargeTime = 2.0f;
        private float _chargeTime = 0f;

        #region Properties
        //we need to be able to access outside this class; ie for updating the slider value UI element
        public float chargeTime { get { return _chargeTime; } }
        #endregion

        #region Unity Events
        protected override void Update()
        {
            base.Update();
        }

        #endregion

        #region Overloaded NVR Methods
        // Overload the NVR Button pressed. WHen button is being ressed we need to add to the time and keep it in a certain range
        public override void UseButtonPressed()
        {
            _chargeTime += Time.deltaTime;

            _chargeTime = Mathf.Clamp(_chargeTime, 0.0f, maxChargeTime);
        }

        //Only shoot the gun when the button is released. Hence the overload
        public override void UseButtonUp()
        {
            //only shoot when it is charged enough
            if (_chargeTime >= maxChargeTime)
            {
                shootGun();
            }

            // Everytime the trigger is released, we need to reset the timer
            _chargeTime = 0f;
        }

        #endregion


    } // end of Charge class

}
