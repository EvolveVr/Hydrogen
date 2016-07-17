using UnityEngine;
using UnityEngine.UI;
using NewtonVR;

namespace Hydrogen
{
    /// <summary>
    /// BASE CLASS FOR ALL WEAPONS; It will contain basic information liek the name of the
    /// weapon and the 
    /// </summary>
    
    public abstract class Weapon : NVRInteractableItem
    {
        [Header("Weapon Class Fields")]
        [Space(15)]
        public string WeaponName;
        public string _weaponDescription;               //change to private later
        private Image _weaponImage;
        public GameConstants.WeaponType weaponType;

        #region Properties
        public string weaponDescription { get { return _weaponDescription; } }

        public Image weaponImage
        {
            get { return _weaponImage; }
            set { _weaponImage = value; }
        }

        public string weaponName { get { return WeaponName; } }

        #endregion

        // This is used to initialize the weapon image. Every weapon needs this function
        public void initWeaponImage()
        {
            _weaponImage = GetComponent<Image>();
        }
    }
}
