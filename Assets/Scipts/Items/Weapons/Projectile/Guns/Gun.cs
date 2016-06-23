using UnityEngine;
using NewtonVR;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// Abstract class Gun will hold all the methods, properties and Fields that 
    /// are general to ALL guns in the game
    /// </summary>
    
    public class Gun : ProjectileWeapon
    {
        // Leave this out for now
        /*
        //public Magazine currentMagazine;
        //public float secondsAfterDetach = 0.2f;
        //public Transform magazinePosition;
        */

        //The gun is ready to fire; all guns need some sort of engagement whether it happens
        // automatically or it needs user interaction
        private bool _isEngaged = false;
        public Transform firePoint;
        public Bullet bulletPrefab;
        
        #region HAPTIC FEEDBACK VARS
        //In general, all the Guns will have some sort of haptic feedback at fire'
        [HideInInspector]
        public int VibrationCount = 1;
        [HideInInspector]
        public float VibrationLength = 1000f;
        [HideInInspector]
        public float VibrationStrength = 1;
        [HideInInspector]
        public float gapLength = 0.01f;

        #endregion

        #region Properties
        
        /*
        public bool isLoaded
        {
            get { return currentMagazine != null; }
        }
        */

        public bool isEnggaed
        {
            get { return _isEngaged; }
            set { _isEngaged = value; }
        }

        #endregion

        #region Methods

        protected override void Update()
        {
            base.Update();



            /*
             * This commented region of code was used for dropping clips when touch pad was pressed
             * This is commented because future guns may not necessarily have clips;
                if (AttachedHand != null)
                {
                    if (AttachedHand.Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                    {
                        dropCurrentMagazine();
                    }
                }
            */
        }

        // Commented region of code for droping magazines
        //protected void dropCurrentMagazine()
        //{
        //    if (isLoaded)
        //    {
        //        StartCoroutine(disableMagazineCollider());
        //        currentMagazine.attachMagazine(false);
        //        currentMagazine = null;

        //    }

        //    isEngaged = false;
        //}

        public override void UseButtonDown()
        {
            shootGun();
        }

        //All guns need to shoot;
        protected void shootGun()
        {
            bulletPrefab = Instantiate(bulletPrefab);
            bulletPrefab.transform.position = firePoint.position;
            bulletPrefab.transform.rotation = firePoint.rotation;
            bulletPrefab.GetComponent<Bullet>().initialize();

            /*
            StartCoroutine(LongVibration(VibrationCount, VibrationLength, gapLength, VibrationStrength));
            StopCoroutine(LongVibration(VibrationCount, VibrationLength, gapLength, VibrationStrength));
            */
        }
        
        // This is for guns with magazines
        /*
        public IEnumerator disableMagazineCollider()
        {
            magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(secondsAfterDetach);
            magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = true;
            StopCoroutine(disableMagazineCollider());
        }
        */

        //All guns will have some sort of haptics
        //vibrationCount is how many vibrations
        //vibrationLength is how long each vibration should go for
        //gapLength is how long to wait between vibrations
        //strength is vibration strength from 0-1
        public IEnumerator LongVibration(int vibrationCount, float vibrationLength, float gapLength, float strength)
        {
            strength = Mathf.Clamp01(strength);
            for (int i = 0; i < vibrationCount; i++)
            {
                if (i != 0) yield return new WaitForSeconds(gapLength);
                yield return StartCoroutine(LongVibration(vibrationLength, strength));
            }
        }

        IEnumerator LongVibration(float length, float strength)
        {
            for (float i = 0; i < length; i += Time.deltaTime)
            {
                if (AttachedHand != null)
                    AttachedHand.Controller.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
                yield return null;
            }
        }

        #endregion
    }
}