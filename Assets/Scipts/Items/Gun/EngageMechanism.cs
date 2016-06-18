using UnityEngine;
using System.Collections;
using NewtonVR;


namespace Hydrogen
{
    public class EngageMechanism : MonoBehaviour
    {
        public float controllerVectorMagnitude = 1.5f;

        // the top piece of the gun updates its position
        void UpdateEngagePiecePosition()
        {

        }

        // While the controller is inside the boxcoliider we 1) get gun script attached to this object
        // 2) then we get the controller coming into the box collider 3) check to see if the velocity meets 
        // minimum velocity for engaging
        void OnTriggerStay(Collider other)
        {
            NVRHand attachedHand = GetComponentInParent<SemiAutomaticGun>().AttachedHand;

            //is a controller currently interacting with this gun?
            if(attachedHand != null)
            {
                NVRHand otherHand = other.GetComponentInParent<NVRHand>();

                if (otherHand != null)
                {
                    if(otherHand.GetVelocityEstimation().sqrMagnitude >= controllerVectorMagnitude)
                    {
                        //engage the weapon here
                        GetComponentInParent<SemiAutomaticGun>().isEngaged = true;
                    }
                }
            }
        }

    }
}