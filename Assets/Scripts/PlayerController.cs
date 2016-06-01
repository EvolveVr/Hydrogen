using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private GameObject myWeapon;
    public GameObject weapon
    {
        get { return myWeapon; }
        set
        {
            myWeapon = value;
            hideViveController(false);
        }
    }

    public GameObject controllerV;

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObject.index); } }

    // Buttons on controller references and booleans for when they are and are not being pressed
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

   
    // Haptic feedback
    public float hapticIntensity = 1500f;
    public float hapticTime = 0.1f;
    public float hapticStrength = 0.1f;

    public bool isTriggerClicked = false;
    
	// Use this for initialization
	void Start () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();

       if (trackedObject == null)
        {
            enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // check to see if the controller is null, and not being detected
        if (controller == null)
        {
            //Debug.Log("Controller is null");
            return;
        }
       
        if(controller.GetPressDown(triggerButton))
        {
            //Debug.Log("Trigger Button Just presseed!");
            isTriggerClicked = true;
            Debug.Log("TRIGGER IS CLICKED ");
            if (weapon != null)
            {
                if (weapon.GetComponent<Gun>() != null)
                {
                    weapon.GetComponent<Gun>().shoot();
                }
            }
        }
        if (controller.GetPress(triggerButton))
        {
        //    Debug.Log("Trigger Button Just presseed!");
            isTriggerClicked = true;
            
        }
        if (controller.GetPressUp(triggerButton))
        {
           // Debug.Log("Trigger Button Just released!");
            isTriggerClicked = false;
           // Debug.Log("CLICKED" + isTriggerClicked);
        }
        if (controller.GetPressDown(gripButton))
        {
            //Debug.Log("Trigger Button Just presseed!");
            if (weapon != null)
            {
                if(weapon.GetComponent<Gun>() != null){

                    weapon.GetComponent<Gun>().dropMagazine();
                }
            }
        }
        if (controller.GetPress(gripButton))
        {
            //Debug.Log("Trigger Button being held!");
        }
        if (controller.GetPressUp(gripButton))
        {
            //Debug.Log("Trigger Button Just released!");
        }

    }

    // these function are for te Rumble effect of the controller
    void RumbleController(float duration, float strength)
    {
        StartCoroutine(RumbleControllerRoutine(duration, strength));
    }

    IEnumerator RumbleControllerRoutine(float duration, float strength)
    {
        strength = Mathf.Clamp01(strength);
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime <= duration)
        {
            int valveStrength = Mathf.RoundToInt(Mathf.Lerp(0, 3999, strength));

            controller.TriggerHapticPulse((ushort)valveStrength);

            yield return null;
        }
    }


    public void hideViveController(bool hide)
    {
        controllerV.SetActive(hide);
    }

    //trigger event
    /*void OnTriggerEnter(Collider enteringObject)
    {
        if (controller == null)
            return;

        Debug.Log("controller entered gun");

        if (enteringObject == gun.GetComponent<Collider>())
        {
            gun.gameObject.transform.parent = this.gameObject.transform;
        }
    }*/
}
