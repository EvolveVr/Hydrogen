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
        #region GUN
        //can continuously shoot bullets without reload
        public bool isInfinite;
        //whether the gun needs to be engaged AFTER reloading only
        public bool needsEngagment = false;
        //true if the gun is automatic false if the gun is semi automatic
        public bool isAutomatic;
        #endregion

        #region AUTOMATIC VARIABLES
        public float timeBetweenShots = 0.5f;
        public float timeSinceLastShot = 0;
        #endregion

        public bool isEngaged = false;
        public Transform firePoint;

        #region MAGAZINE VARIABLES
        public Magazine currentMagazine;
        public float secondsAfterDetach = 0.2f;
        public Transform magazinePosition;
        #endregion

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
        // isLoaded means there is a magazine
        public bool isLoaded
        {
            get { return currentMagazine != null; }
        }

        // Below are properties for whther the user is pressing the left or right pad buttons
        public bool rightPadButtonDown
        {
            get
            { return  AttachedHand.Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) &&
                          (AttachedHand.Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)[0] > 0.05f);
            }
        }

        public bool leftPadButtonDown
        {
            get
            {
                return AttachedHand.Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) &&
                            (AttachedHand.Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)[0] < 0.05f);
            }
        }

        #endregion

        #region Methods
        protected override void Update()
        {
            base.Update();

            //only drop magazine if the gun currently has a magazine and does not shoot infinitly
            if (AttachedHand != null && !isInfinite)
            {
                if (rightPadButtonDown)
                {
                    dropCurrentMagazine();
                }

                //The gun needs to be loaded, and needs to have bullets and should not already be engaged to engage it
                if (leftPadButtonDown)
                {
                    if (isLoaded && currentMagazine.hasBullets && !isEngaged)
                    {
                        isEngaged = true;
                    }
                }
            }        
        }
        

        protected void dropCurrentMagazine()
        {
            if (isLoaded)
            {
                StartCoroutine(disableMagazineCollider());
                currentMagazine.attachMagazine(false);
                currentMagazine = null;
            }

            isEngaged = false;
        }

        public override void UseButtonPressed()
        {
            if (isAutomatic && isLoaded)
            {
                if (needsEngagment)
                {
                    if (isEngaged)
                    {
                        if(timeSinceLastShot >= timeBetweenShots)
                        {
                            shootGun();
                            timeSinceLastShot = 0;
                        }
                        timeSinceLastShot += Time.deltaTime;
                    }
                }

                else
                {
                    if (timeSinceLastShot >= timeBetweenShots)
                    {
                        shootGun();
                        timeSinceLastShot = 0;
                    }

                    timeSinceLastShot += Time.deltaTime;
                }
            }
        }

        public override void UseButtonDown()
        {
            if (isLoaded)
            {
                if (needsEngagment)
                {
                    if (isEngaged)
                    {
                        shootGun();
                    }
                }
                else
                {
                    shootGun();
                }
            }
        }

        public override void UseButtonUp()
        {
            if (isAutomatic)
            {
                timeSinceLastShot = 0;
            }
        }

        protected void shootGun()
        {
            if (currentMagazine.hasBullets)
            {
                GameObject tempBullet = currentMagazine.getBullet;
                tempBullet.transform.position = firePoint.position;
                tempBullet.transform.rotation = firePoint.rotation;
                tempBullet.GetComponent<Bullet>().initialize();
                tempBullet = null;
            }
        }
        
        // This is for guns with magazines
        public IEnumerator disableMagazineCollider()
        {
            magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(secondsAfterDetach);
            magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = true;
            StopCoroutine(disableMagazineCollider());
        }
        
        /*
        //All guns will have some sort of haptics
        //vibrationCount is how many vibrations
        //vibrationLength is how long each vibration should go for
        //gapLength is how long to wait between vibrations
        //strength is vibration strength from 0-1
        */
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