using UnityEngine;
using System.Collections;

public class AttachToJoint : MonoBehaviour
{
    public GameObject joint;
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.position = joint.transform.position;
	}
}
