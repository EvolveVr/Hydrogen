﻿using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    public class MagazineCollider : MonoBehaviour
    {
        public GameObject parentGun;

        void OnTriggerEnter(Collider other)
        {
            Magazine collidingMag = other.GetComponent<Magazine>();
            if (collidingMag != null && !collidingMag.isEquipped)
            {
                //means a magazine is currently touching the collider
                collidingMag.attachMagazine(true, parentGun);
            }
        }
    }
}

