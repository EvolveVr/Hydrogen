using UnityEngine;
using NewtonVR;


public class RestrictedMovement : MonoBehaviour
{
    private Rigidbody rb;
    public NVRHand hand;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(hand.Controller != null)
        {
            if(hand.Controller.GetHairTrigger())
            {
                Vector3 vectTowardController = (hand.transform.position - this.transform.position);
                Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, hand.transform.position.z);
                transform.position = newPosition;
            }
        }
        
    }
	
}
