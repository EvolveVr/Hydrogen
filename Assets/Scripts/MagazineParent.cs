using UnityEngine;
using System.Collections;

public class MagazineParent : MonoBehaviour {
    public Gun myGunParent;

    void OnTriggerEnter(Collider enteringObject)
    {
        if (enteringObject.GetComponent<PlayerController>() != null)
        {
            if(enteringObject.GetComponentInChildren<Magazine>() != null)
            {
                Magazine myMag = enteringObject.GetComponentInChildren<Magazine>();
                if (!myGunParent.isLoaded)
                {
                    Debug.Log("MAGAZINE IS TOUCHING ME");
                    myGunParent.equip(myMag);
                }
            }
            
            /*
            TODO: FOR NOW, ITEMS WILL BE COLLECTED AS SOON AS YOU RUN INTO THEM
            WE NEED TO MAKE IT SO WHEN TRIGGER CLICKED AND ITEM IS CURRENTLY COLLIDING - THEN PICK UP
            VIBRATE CONTROLLER
            */
        }
    }
}
