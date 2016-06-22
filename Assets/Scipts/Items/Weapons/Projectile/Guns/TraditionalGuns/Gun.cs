using UnityEngine;
using NewtonVR;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// Abstract class Gun will hold all the methods, properties and Fields that 
    /// are general to ALL guns in the game
    /// </summary>
    public abstract class Gun : ProjectileWeapon
    {
        public Magazine currentMagazine;
        public float secondsAfterDetach = 0.2f;
        
        //the gun is ready to fire
        public bool isEngaged = false;
        //the gun has a clip in it
        public bool isLoaded
        {
            get { return currentMagazine != null; }
        }

        public Transform firePoint;
        public Transform magazinePosition;

        #region HAPTIC FEEDBACK VARS
        public int VibrationCount = 1;
        public float VibrationLength = 1000f;
        public float VibrationStrength = 1;
        public float gapLength = 0.01f;
        #endregion

        protected override void Update()
        {
            base.Update();
            if (AttachedHand != null)
            {
                if (AttachedHand.Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    dropCurrentMagazine();
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
            shootGun();
        }

        //All guns need to shoot, but some of them shoot in very different ways;
        protected void shootGun()
        {
            if (isLoaded)
            {
                if (currentMagazine.hasBullets && isEngaged)
                {
                    GameObject bullet = currentMagazine.getBullet;
                    bullet.transform.position = firePoint.position;
                    bullet.transform.rotation = firePoint.rotation;
                    StartCoroutine(LongVibration(VibrationCount, VibrationLength, gapLength, VibrationStrength));
                    bullet.GetComponent<Bullet>().initialize();
                    StopCoroutine(LongVibration(VibrationCount, VibrationLength, gapLength, VibrationStrength));
                }
            }
        }
        
        protected void playGunShotSound() { }

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

        IEnumerator LongVibration(float length, float strength)
        {
            for (float i = 0; i < length; i += Time.deltaTime)
            {
                if (AttachedHand != null)
                    AttachedHand.Controller.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
                yield return null;
            }
        }
    }
}