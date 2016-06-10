using UnityEngine;
using NewtonVR;
using System.Collections.Generic;


namespace Hydrogen
{
    // loaded measn there is a clip in it
    // equipped measn that your holding the gun
    public abstract class Gun : NVRInteractableItem
    {
        // current and max number of bullets
        public Magazine _currentMagazine;
        public Transform firePoint;
        public Transform magazinePosition;

        protected int _maxNumberOfBullets;
        protected int _currentNumberOfBullets;
        

        // Public Methods
        public void isEquipped() { }
        public void isEngaged() { }

        bool isLoaded
        {
            get { return _currentMagazine != null; }
        }

        protected void shootGun()
        {
            if (isLoaded)
            {
                if (_currentMagazine.hasBullets)
                {
                    GameObject bullet = _currentMagazine.getBullet;
                    bullet.transform.position = firePoint.position;
                    bullet.transform.rotation = firePoint.rotation;
                    bullet.GetComponent<Bullet>().addForce();
                }
            }
        }

        protected void applySpreadToBullet() { }

        protected void dropCurrentMagazine()
        {
            if(isLoaded)
            {
                _currentMagazine.attachMagazine(false);
                _currentMagazine = null;
            }
        }

        protected void reloadGun() { }

        protected void updateEngageLevel() { }

        protected void playGunShotSound() { }

        protected void playEngageSound() { }

        protected void playNonEngagedSound() { }

        protected float getControllerTriggerPosition() { return 0.0f; }

        protected void updateTriggerPosition() { }

        public void updateEngageLeverPosition() { }

        public override void UseButtonDown()
        {
            shootGun();
        }

        protected override void Update()
        {
            base.Update();
            if (AttachedHand != null)
            {
                if(AttachedHand.Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    dropCurrentMagazine();
                }
            }
        }

    }
}
