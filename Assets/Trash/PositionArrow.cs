using UnityEngine;
using NewtonVR;
using System.Collections;

public class PositionArrow : MonoBehaviour
{
    private NVRHand attahcedHand;
    private BoxCollider boxCollider;

    public Transform stringTransform;
    public Transform arrowPosition;
    public float rateOfArrowPositioning = 45.0f;
    public StringPull stringPullScript;
	
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Arrow")
        {
            attahcedHand = other.GetComponent<NVRInteractableItem>().AttachedHand;

            if (attahcedHand != null)
            {
                if(attahcedHand.Controller.GetHairTrigger())
                {
                    other.transform.SetParent(stringTransform);
                    other.transform.position = arrowPosition.position;
                    other.transform.rotation = arrowPosition.rotation;
                    other.GetComponent<Rigidbody>().useGravity = false;
                    attahcedHand.EndInteraction(other.GetComponent<NVRInteractableItem>());
                    boxCollider.enabled = false;
                    stringPullScript.enabled = true;
                }
                else
                {
                    positionArrow(other.gameObject);
                }
            }
        }
    }

    void positionArrow(GameObject arrow)
    {
        arrow.transform.position = Vector3.Lerp(arrow.transform.position, arrowPosition.position, rateOfArrowPositioning * Time.deltaTime);
    }

}
