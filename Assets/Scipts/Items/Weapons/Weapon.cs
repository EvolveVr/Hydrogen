﻿using UnityEngine;
using UnityEngine.UI;
using NewtonVR;

namespace Hydrogen
{
    /// <summary>
    /// BASE CLASS FOR ALL WEAPONS
    /// </summary>
    public abstract class Weapon : NVRInteractableItem
    {
        [Header("Fields below are from Weapon Class")]
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

        public void initWeaponImage()
        {
            _weaponImage = GetComponent<Image>();
        }
    }
}
