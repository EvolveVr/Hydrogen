using UnityEngine;
using NewtonVR;
using System.Collections;
namespace Hydrogen
{
    public class SlideEngage : MonoBehaviour
    {
        private Vector3 engageStart;
        private Vector3 engageEnd;
        public bool engageHitEnd;
        public bool engaged = false;
        public bool isHeld = false;
        public float time = 0.1f; //time in seconds for the engage to reset from any position
        public float holdStartPosition;
        public float unheldTime;
        public NVRHand holdingHand;

        public delegate void callback();
        callback onEngage;
        callback onInitial;

        void Start()
        {
            engageStart = transform.localPosition;
            engageEnd = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.15f);
        }

        void Update()
        {
            if (isHeld)
            {
                Vector3 temp = transform.InverseTransformPoint(holdingHand.transform.position);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, holdStartPosition + temp.z);

                if (transform.localPosition.z < engageEnd.z)
                {
                    if (engageHitEnd == false)
                    {
                        engageHitEnd = true;
                        //initial hit of engage end
                        if (onInitial != null) onInitial();
                    }
                    transform.localPosition = engageEnd;
                }

                if (transform.localPosition.z > engageStart.z)
                {
                    if (engageHitEnd == true)
                    {
                        engageHitEnd = false;
                        //engagement is finished
                        if (onEngage != null) onEngage();
                    }
                    transform.localPosition = engageStart;
                }
            }
            else
            {
                if (transform.localPosition.z < engageStart.z)
                {

                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(holdStartPosition, engageStart.z, (Time.time - unheldTime) / time));
                }
                else if (engageHitEnd)
                {
                    engageHitEnd = false;
                    //engagement is finished
                    if (onEngage != null) onEngage();
                }
            }
        }


        void OnTriggerStay(Collider other)
        {
            holdingHand = other.GetComponentInParent<NVRHand>();

            if (holdingHand)
            {
                if (holdingHand.UseButtonPressed)
                {
                    Vector3 temp = transform.InverseTransformPoint(holdingHand.transform.position);
                    if (!isHeld)
                    {
                        isHeld = true;
                        holdStartPosition = transform.localPosition.z - temp.z;
                    }

                }
                else
                {
                    unheld();
                }
            }
            else
            {
                unheld();
            }
        }

        void OnTriggerExit()
        {
            unheld();
        }

        private void unheld()
        {
            holdStartPosition = transform.localPosition.z;
            unheldTime = Time.time;
            isHeld = false;
        }


        public void setEngage(callback test)
        {
            onEngage = test;
        }

    }
}