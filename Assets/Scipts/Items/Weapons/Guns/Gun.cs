using UnityEngine;
using UnityEngine.UI;
using NewtonVR;
using System.Collections;
using System;

namespace Hydrogen
{
    /// <summary>
    /// Abstract class Gun will hold all the methods, properties and Fields that 
    /// are general to ALL guns in the game
    /// </summary>
    // Because all guns will have to have a sprite to accompany the weapon for UI purposes
    [RequireComponent(typeof(Image))]
    public class Gun : Weapon
    {
        #region GUN Flags

        [Header("Fields below from Gun class")]
        [Space(15)]
        //can continuously shoot bullets without reload
        [HideInInspector]
        private bool _isInfinite = true;
        //whether the gun needs to be engaged AFTER reloading only
        [HideInInspector]
        public bool needsEngagment = false;
        //true if the gun is automatic false if the gun is semi automatic
        [HideInInspector]
        public bool isAutomatic;
        // is the gun a charge type weapon
        [HideInInspector]
        public bool isCharge = false;
        //true if the gun needs to re-engage after every shot false otherwise
        [HideInInspector]
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

        public bool IsInfinite
        {
            get { return _isInfinite; }
            set { _isInfinite = value; }
        }

        #endregion

        #region Unity Methods
        protected override void Start()
        {
            //before doing anything, we should initialize the flag values for the gun or next statement will not work
            initWeaponFlags(GameConstants.gunTypeInitValues[weaponType]);

            base.Start();
            if (needsEngagment)
            {
                SlideEngage = GetComponentInChildren<SlideEngage>();
                if(SlideEngage!= null)
                    SlideEngage.setEngage(engageGun);
            }

            //We need to initialize the image
            initWeaponImage();
        }


        protected override void Update()
        {
            base.Update();

            //only drop magazine if the gun currently has a magazine and does not shoot infinitly
            if (AttachedHand != null)
            {
                if (rightPadButtonDown)
                {
                    if (!IsInfinite)
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
        #endregion

        #region NVR Overrides
        public override void UseButtonDown()
        {
            // do not do this if the gun uses a charging mechanism instead
            if (!isCharge)
            {
                gunMechanics();
            }
        }

        //UseButtonPressed is when the button is being held down
        // thus, this only works if your na automatic weapon
        public override void UseButtonPressed()
        {
            if (isAutomatic) autoGunMechanics();
        }

        //When an autonmatic is shooting, we need to reset the time because 
        // if they release the trigger being mid shot, we dont want it to shoot early next time they shoot
        public override void UseButtonUp()
        {
            if (isAutomatic)
            {
                timeSinceLastShot = 0;
            }
        }
        #endregion

        #region Methods
        void initWeaponFlags(GunTypeInitValues _gunInitValues)
        {
            IsInfinite = _gunInitValues.isInfinite;
            isAutomatic = _gunInitValues.isAutomatic;
            isCharge = _gunInitValues.isCharge;
            needsEngagment = _gunInitValues.needsEngagment;
            isRepeater = _gunInitValues.isRepeater;
        }

        //Temporary
        private void engageGun()
        {
            isEngaged = true;
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
        #endregion


    } // End of gun class


    //this will allow us to automate the flag value assigning for each diff. type of weapon
    public struct GunTypeInitValues
    {
        public bool isInfinite;
        public bool isAutomatic;
        public bool isCharge;
        public bool isRepeater;
        public bool needsEngagment;

        // constructor
        public GunTypeInitValues
            (bool _isInfinite = false, bool _isAutomatic = false, bool _isRepeater = false, bool _needsEnagement = false, bool _isCharge = false)
        {
            this.isInfinite = _isInfinite;
            this.isAutomatic = _isAutomatic;
            this.isRepeater = _isRepeater;
            this.needsEngagment = _needsEnagement;
            this.isCharge = _isCharge;
        }
    }

}