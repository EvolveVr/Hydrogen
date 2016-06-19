using UnityEngine;
using NewtonVR;
using System.Collections;


namespace Hydrogen
{
    // Loaded means there is a clip in it;
    // Equipped means that your holding the gun;
    // Engaged means it is ready to fire
    public abstract class Gun : NVRInteractableItem
    {
        public bool _isEngaged = false;

        // current and max number of bullets
        public GunType gunType;
        public Transform firePoint;

        //haptics; all guns have a haptic feedback after shooting
        public int VibrationCount = 1;
        public float VibrationLength = 1000f;
        public float VibrationStrength = 1;
        public float gapLength = 0.01f;

        //Properties

        public GunType GunType
        {
            get
            {
                return gunType;
            }
        }

        public bool isEngaged
        {
            get { return _isEngaged; }

            set { _isEngaged = value; }
        }

        protected abstract void shootGun();

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


        // All unimplemented methods

        protected void playGunShotSound() { }

        //protected float getControllerTriggerPosition() { return 0.0f; } // might be for all guns
        // protected void updateTriggerPosition() { } // might be for all guns
    }
}
