using UnityEngine;
using System.Collections;

public class pickupItem : MonoBehaviour {
    public bool parent = false;
    public Transform myTrans;
    
    public void Awake()
    {
        myTrans = GetComponent<Transform>();
    }

    //trigger event
    void OnTriggerEnter(Collider enteringObject)
    {
        if (parent) { return; }

        if (enteringObject.GetComponent<WandController>() != null)
        {
            parent = true;
            Transform temp = enteringObject.GetComponent<Transform>();
            myTrans.SetParent(temp);
            myTrans.position = new Vector3(temp.position.x, temp.position.y, temp.position.z);
            transform.eulerAngles = new Vector3(temp.eulerAngles.x, temp.eulerAngles.y, -temp.eulerAngles.z);
        }
    }
}
