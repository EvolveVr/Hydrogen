using UnityEngine;
using System.Collections;
using NewtonVR;

namespace Hydrogen
{
    /// <summary>
    /// BASE CLASS FOR ALL WEAPONS
    /// </summary>
    public abstract class Weapon : NVRInteractableItem
    {
        public string weaponName;
        public string weaponDescription;
        public bool projectileWeapon;
    }
}
