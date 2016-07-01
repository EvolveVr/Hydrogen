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
        public bool isInfinite = true;
        //whether the gun needs to be engaged AFTER reloading only
        public bool needsEngagment = false;
        //true if the gun is automatic false if the gun is semi automatic
        public bool isAutomatic;
        // is the gun a charge type weapon
        public bool isCharge = false;
        //true if the gun needs to re-engage after every shot false otherwise
        public bool isRepeater = false;
        #endregion

        #region AUTOMATIC VARIABLES
        public float timeBetweenShots = 0.5f;
        public float timeSinceLastShot = 0;
        #endregion

        public bool isEngaged = false;
        public SlideEngage SlideEngage;
        public Transform firePoint;

        #region MAGAZINE VARIABLES
        public Magazine currentMagazine;
        public float secondsAfterDetach = 0.2f;
        public Transform magazinePosition;
        #endregion

        #region HAPTIC FEEDBACK VARS
        float VibrationLength = 0.1f;
        ushort VibrationIntensity = 2000;
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
        protected override void Start()
        {
            base.Start();
            if (needsEngagment)
            {
                SlideEngage = GetComponentInChildren<SlideEngage>();
                if(SlideEngage!= null)
                    SlideEngage.setEngage(engageGun);
            }
        }

        //Temporary
        private void engageGun()
        {
            isEngaged = true;
        }


        protected override void Update()
        {
            base.Update();

            //only drop magazine if the gun currently has a magazine and does not shoot infinitly
            if (AttachedHand != null)
            {
                if (rightPadButtonDown)
                {
                    if (!isInfinite)
                    {
                        dropCurrentMagazine();
                    }
                }

                //The gun needs to be loaded, and needs to have bullets and should not already be engaged to engage it
                if (leftPadButtonDown)
                {
                    if (needsEngagment)
                    {
                        if (isLoaded && !isEngaged)
                        {
                            isEngaged = true;
                        }
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


        public override void UseButtonDown()
        {
            // do not do this if the gun uses a charging mechanism instead
            if (!isCharge)
            {
                gunMechanics();
            }
        }

        public override void UseButtonPressed()
        {
            if (isAutomatic) autoGunMechanics();
        }

        public override void UseButtonUp()
        {
            if (isAutomatic)
            {
                timeSinceLastShot = 0;
            }
        }

        virtual protected void shootGun()
        {
            if (currentMagazine.hasBullets)
            {
                GameObject tempBullet = currentMagazine.getBullet;
                tempBullet.transform.position = firePoint.position;
                tempBullet.transform.rotation = firePoint.rotation;
                tempBullet.GetComponent<Bullet>().initialize();
                tempBullet = null;
                AttachedHand.LongHapticPulse(VibrationLength, VibrationIntensity);

                if (isRepeater)
                {
                    isEngaged = false;
                }
            }
        }
        
        public IEnumerator disableMagazineCollider()
        {
            magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(secondsAfterDetach);
            magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = true;
            StopCoroutine(disableMagazineCollider());
        }
        #endregion

        // the mechanics for the automatic guns
        void autoGunMechanics()
        {
            if (isLoaded)
            {
                if (needsEngagment)
                {
                    if (isEngaged)
                    {
                        if (timeSinceLastShot >= timeBetweenShots)
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
        } //for automatic guns only

        //the mechanics for non-autmatic guns
        void gunMechanics()
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
        }// gun gun mechanics


    } // End of gun class
}