using UnityEngine;
using System.Collections;


namespace Hydrogen
{ 
    /// <summary>
    /// Mostly used for makeing sure player doesnt loss there gun
    /// </summary>
    public class Antigravity : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Weapon")
            {
                Rigidbody gunRB = other.GetComponent<Rigidbody>();
                gunRB.velocity = Vector3.zero;
                gunRB.useGravity = false;
                gunRB.angularDrag = 1.3f;
            }
        }
    }
}
