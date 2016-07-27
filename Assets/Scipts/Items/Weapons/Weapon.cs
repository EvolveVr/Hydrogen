using UnityEngine;
using UnityEngine.UI;
using NewtonVR;

namespace Hydrogen
{
    /// <summary>
    /// BASE CLASS FOR ALL WEAPONS;
    /// Every weapon contains: 1) name/description/image 2) a weapon type and corresponding flags for type, 
    /// 3) a fire point, where bullet is instantiated, 4) Haptic feedback when fired, 5) and methods for:
    /// shooting, sound, animation, and particle effects
    /// </summary>
    
    public abstract class Weapon : NVRInteractableItem
    {
        [Header("Weapon Class Fields")]
        [Space(15)]
        public string _weaponName;
        public string _weaponDescription;
        private Image _weaponImage;

        //we want to set this in inspector
        public GameConstants.WeaponType weaponType;

        //Every weaon has these gun tags
        #region GUN Flags
        [Space(15)]
        //whether the gun needs to be engaged AFTER reloading only
        [HideInInspector]
        public bool needsEngagment = false;
        private bool _isEngaged = false;
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

        // every weapon has a fire point; and a prefab for the bullet
        public Transform firePoint;
        public GameObject bulletPrefab;

        // every weapon has haptic feedback
        #region HAPTIC FEEDBACK VARS
        protected float VibrationLength = 0.1f;
        protected ushort VibrationIntensity = 2000;
        #endregion

        #region Properties
        public string weaponDescription { get { return _weaponDescription; } }
        public Image weaponImage
        {
            get { return _weaponImage; }
            set { _weaponImage = value; }
        }
        public string weaponName { get { return _weaponName; } }

        // Below are propertie for returning whether pad button is being pressed; for engagement
        public bool padButtonDown
        {
            get
            {
                if(AttachedHand != null)
                    return AttachedHand.Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

                return false;
            }
        }

        //every weapon has its type
        public GameConstants.WeaponType WeaponType
        {
            get { return weaponType; }
            set { weaponType = value; }
        }

        //only children should be setting this variable, and only guns that need engagement should use this
        protected bool isEngaged
        {
            get { return _isEngaged; }
            set { _isEngaged = value; }
        }
        #endregion

        #region Methods
        // Every weapon needs a few things loaded;
        //1) weapon type initialization, 2) 
        protected void initWeapon()
        {
            initWeaponFlags(GameConstants.gunTypeInitValues[weaponType]);
            _weaponImage = GetComponent<Image>();
            firePoint = transform.FindChild("FirePoint");
        }

        //depending on the type, every weapon needs to call this method in Awake()
        protected void initBulletPrefab(string bulletPrefabPath)
        {
            bulletPrefab = Resources.Load(bulletPrefabPath) as GameObject;
        }

        //every weapon needs to fire
        protected virtual void shootWeapon()
        {
            //1) instantiate prefab, put it in correct position and rotation, init bullet; a;; bullets have bullet class
            Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.initialize();

            //instantiate the bullet particle effect

            //Play sound

            AttachedHand.LongHapticPulse(VibrationLength, VibrationIntensity);
        }


        //When class inheriting from this class is instantiated, initialize the gun flags
        void initWeaponFlags(GunTypeInitValues _gunInitValues)
        {
            isAutomatic = _gunInitValues.isAutomatic;
            isCharge = _gunInitValues.isCharge;
            needsEngagment = _gunInitValues.needsEngagment;
            isRepeater = _gunInitValues.isRepeater;
        }
        #endregion
    }

    //this will allow us to automate the flag value assigning for each diff. type of weapon
    public struct GunTypeInitValues
    {
        public bool isAutomatic;
        public bool isCharge;
        public bool isRepeater;
        public bool needsEngagment;

        // constructor
        public GunTypeInitValues
            (bool _isAutomatic = false, bool _isRepeater = false, bool _needsEnagement = false, bool _isCharge = false)
        {
            this.isAutomatic = _isAutomatic;
            this.isRepeater = _isRepeater;
            this.needsEngagment = _needsEnagement;
            this.isCharge = _isCharge;
        }
    }

}
