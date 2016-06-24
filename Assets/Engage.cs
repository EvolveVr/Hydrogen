using UnityEngine;
using NewtonVR;
using System.Collections;

public class Engage : MonoBehaviour {

    public Vector3 engageStart;
    public Vector3 engageEnd;
    public bool engageHitEnd;
    public bool engaged = false;

    public delegate void callback();
    callback onEngage;
    callback onInitial;

    void Start()
    {
        engageStart = transform.localPosition;
        engageEnd = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.15f);
    }

    void OnTriggerStay(Collider other)
    {
        NVRHand myHand = other.GetComponentInParent<NVRHand>();

        if (myHand)
        {
            if (other.GetComponentInParent<NVRHand>().UseButtonPressed)
            {
                //TODO: FIX THIS SHIT

                /* If Cristian's idea doesn't work, I'll try and fix this up.
                float degree = Vector3.Angle(GetComponentInParent<Transform>().eulerAngles + (Vector3.up*90), myHand.transform.position-GetComponentInParent<Transform>().position);
                float relPos = Mathf.Cos(degree) * (myHand.transform.position - GetComponentInParent<Transform>().position).magnitude;

                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -relPos);
                //*/
                //*Cristian's idea
                Vector3 temp = transform.InverseTransformPoint(myHand.transform.position);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -temp.x);
                //*/
                if (transform.localPosition.z < engageEnd.z)
                {
                    if(engageHitEnd == false)
                    {
                        engageHitEnd = true;
                        //initial hit of engage end
                        if (onInitial!=null) onInitial();
                    }
                    transform.localPosition = engageEnd;
                }

                if (transform.localPosition.z > engageStart.z)
                {
                    if(engageHitEnd == true)
                    {
                        engageHitEnd = false;
                        //engagement is finished
                        if(onEngage!=null)onEngage();
                    }
                    transform.localPosition = engageStart;
                }
            }
        }
    }

    void OnTriggerExit()
    {
        if (transform.localPosition.z < engageStart.z&&false)
        {

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(transform.localPosition.z, engageStart.z, 2f));
        }
        else if (engageHitEnd)
        {
            //send engage complete code
            engageHitEnd = false;
        }
    }

    public void setEngage(callback test){
        onEngage = test;
    }

}
