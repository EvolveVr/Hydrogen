using UnityEngine;
using System.Collections;
namespace Hydrogen
{
    /// <summary>
    /// 
    /// </summary>
    public class TraditionalWeapon : Weapon
    {
        // these variables are needed for shot timing
        [Header("TraditionalWeapon Class Fields")]
        [Space(15)]
        #region Automatic and Semiautomatic VARIABLES
        public float timeBetweenShots = 0.5f;
        public float timeSinceLastShot = 0;
        public int numberOfBulletsToEngage = 10;
        private int bulletsShotSinceLastEngage = 0;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            initWeapon();
        }

        #region Unity Methods
        protected override void Update()
        {
            base.Update();
            timeSinceLastShot += Time.deltaTime;

            //if the gun needs engagement then 
            if (needsEngagment)
            {
                if (padButtonDown)
                {
                    isEngaged = true;
                    bulletsShotSinceLastEngage = 0;
                }
            }
        }
        #endregion

        #region NVR Overrides
        public override void UseButtonDown()
        {
            if(isRepeater)
            {
                repeaterGunMechanics();
            }
            else
            {
                gunMechanics();
            }
        }

        //UseButtonPressed is when the button is being held down
        // thus, this only works if your na automatic weapon
        public override void UseButtonPressed()
        {
            if(isAutomatic)
                gunMechanics();
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

        #region Gun Methods
        //only Automatics call this
        void gunMechanics()
        {
            if(needsEngagment)
            {
                if(isEngaged && timeSinceLastShot >= timeBetweenShots && (bulletsShotSinceLastEngage < numberOfBulletsToEngage))
                {
                    shootWeapon();
                    bulletsShotSinceLastEngage++;
                    timeSinceLastShot = 0.0f;
                }
            }
            else
            {
                if (timeSinceLastShot >= timeBetweenShots)
                {
                    shootWeapon();
                    timeSinceLastShot = 0.0f;
                }
            }
        }

  
        void repeaterGunMechanics()
        {
            if (isEngaged && timeSinceLastShot >= timeBetweenShots)
            {
                shootWeapon();
                timeSinceLastShot = 0.0f;
                isEngaged = false;
            }
        }
        #endregion
    }
}
