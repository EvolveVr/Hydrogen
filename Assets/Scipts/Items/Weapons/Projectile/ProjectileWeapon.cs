using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// Any weapon that you have to press/pull something to shoot a projectile
    /// </summary>
    public abstract class ProjectileWeapon : Weapon
    {
        [Header("Field below from ProjectileWeapon class")]
        [Space(15)]
        public bool drawableGun;
    }
}
