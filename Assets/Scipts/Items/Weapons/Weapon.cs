using UnityEngine;
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
        public string weaponName;
        public string weaponDescription;
        public bool projectileWeapon;
    }
}
