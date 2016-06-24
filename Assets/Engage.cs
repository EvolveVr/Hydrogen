using UnityEngine;
using NewtonVR;
using System.Collections;

public class Engage : MonoBehaviour {

    public Vector3 engageStart;
    public Vector3 engageEnd;
    public bool engageHitEnd;
    public bool engaged = false;
    
    void Start()
    {
        engageStart = transform.localPosition;
        engageEnd = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.15f);
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("nvrdzjibvjdak");
        NVRHand myHand = other.GetComponentInParent<NVRHand>();

        if (myHand)
        {
            if (other.GetComponentInParent<NVRHand>().UseButtonPressed)
            {
                //TODO: FIX THIS SHIT
                float degree = Vector3.Angle(GetComponentInParent<Transform>().eulerAngles + (Vector3.up*90), myHand.transform.position-GetComponentInParent<Transform>().position);
                float relPos = Mathf.Cos(degree) * (myHand.transform.position - GetComponentInParent<Transform>().position).magnitude;

                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -relPos);

                if (transform.localPosition.z < engageEnd.z)
                {
                    if(engageHitEnd == false)
                    {
                        engageHitEnd = true;
                        //initial hit of engage end
                    }
                    transform.localPosition = engageEnd;
                }

                if (transform.localPosition.z > engageStart.z)
                {
                    if(engageHitEnd == true)
                    {
                        engageHitEnd = false;
                        //engagement is finished
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
}
