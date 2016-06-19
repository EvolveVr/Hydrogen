using UnityEngine;
using NewtonVR;
using System.Collections;


//Needs lots of work!!
public class Grenade : NVRInteractableItem
{
    public float _seconds = 1.0f;

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<NVRHand>() != null)
        {
            if (other.GetComponent<NVRHand>().UseButtonDown)
            {
                pullPin();
            }
        }
    }

    public void explodeGrenade()
    {
        Debug.Log("Explode!!");
    }

    public void pullPin()
    {
        Debug.Log("Pulling Pin");
        StartCoroutine(startGrenadeCountDown(_seconds));
    }

    IEnumerator startGrenadeCountDown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        explodeGrenade();
    }

}
