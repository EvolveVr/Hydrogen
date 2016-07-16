using UnityEngine;
using UnityEngine.UI;

namespace Hydrogen
{
    /// <summary>
    /// This is a basic class for handling the Sliders for visualizing how 
    /// charged a charged weapon is. This is still a work in progress
    /// </summary>
    public class ChargeWeaponSlider : MonoBehaviour
    {
        //We need a reference to the slider that we will be manipulating and a reference to the charge gun for the max charge time
        // used to set the sliders max value
        public Slider chargeSlider;
        public ChargeGun myChargeGun;

        #region UNITY Events
        // on start, get the slider and the charge gun
        void Start()
        {
            chargeSlider = GetComponentInChildren<Slider>();
            myChargeGun = GetComponentInParent<ChargeGun>();

            //if we successfully got both the charge slider and the charge gun components
            if (chargeSlider)
            {
                chargeSlider.maxValue = myChargeGun.maxChargeTime;
            }
        }

        //The only thing the script really needs to do is update the slider
        void Update()
        {
            chargeSlider.value = myChargeGun.chargeTime;
        }
        #endregion

    }
}
