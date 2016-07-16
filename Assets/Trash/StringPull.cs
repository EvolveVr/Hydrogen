using UnityEngine;
using NewtonVR;
using System.Collections;

public class StringPull : MonoBehaviour
{
    public NVRHand hand;
    private Transform controllerTransform;
    public Rigidbody stringRigidBody;
    public Transform initPostion;
    void Start()
    {
        
        stringRigidBody = GetComponent<Rigidbody>();
        enabled = false;
        stringRigidBody.isKinematic = false;
    }



	void OnTriggerStay(Collider other)
    {
        Debug.Log(other.tag);
        if(other.tag == "ControllerRight" || other.tag == "ControllerLeft" || other.name == "trackhat")
        {

            if (hand.Controller.GetHairTrigger())
            {
                stringRigidBody.isKinematic = false;
                stringRigidBody.AddForce(hand.transform.position - transform.position);
                
            }
            else
                stringRigidBody.isKinematic = true;
        }
    }

    void OnTriggerExit(Collider other)
    {

        transform.position = initPostion.position;
       
    }
}
