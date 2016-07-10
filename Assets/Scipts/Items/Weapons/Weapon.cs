using UnityEngine;
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
        private string _weaponDescription;
        private Sprite _weaponSprite;
        public GameConstants.WeaponType weaponType;
        

        public string weaponDescription { get { return _weaponDescription; } }

        public Sprite weaponSprite { get { return _weaponSprite;} }

        public string weaponName { get { return WeaponName; } }

    }
}
