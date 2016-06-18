using UnityEngine;
using NewtonVR;
using System.Collections;

public class TestVelocityScript : MonoBehaviour
{
    public NVRHand hand;
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(hand.GetVelocityEstimation().sqrMagnitude);
	}
}
