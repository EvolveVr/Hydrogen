using UnityEngine;
using NewtonVR;
using System.Collections;


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

        public float secondsAfterDetach = 0.2f;

        public int VibrationCount = 1;
        public float VibrationLength = 1000f;
        public float VibrationStrength = 1;
        public float gapLength = 0.01f;

        // Public Methods
        public void isEngaged() { }

        public bool isLoaded
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
                    StartCoroutine(LongVibration(VibrationCount, VibrationLength, gapLength, VibrationStrength));
                    StopCoroutine(LongVibration(VibrationCount, VibrationLength, gapLength, VibrationStrength));
                    bullet.GetComponent<Bullet>().addForce();
                }
            }
        }

        protected void applySpreadToBullet() { }

        protected void dropCurrentMagazine()
        {
            if(isLoaded)
            {
                StartCoroutine(disableMagazineCollider());
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

        public IEnumerator disableMagazineCollider()
        {
            magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(secondsAfterDetach);
            magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = true;
            StopCoroutine(disableMagazineCollider());
        }

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

        //length is how long the vibration should go for
        //strength is vibration strength from 0-1
        IEnumerator LongVibration(float length, float strength)
        {
            for (float i = 0; i < length; i += Time.deltaTime)
            {
                if(AttachedHand != null)
                    AttachedHand.Controller.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
                yield return null;
            }
        }

    }
}
