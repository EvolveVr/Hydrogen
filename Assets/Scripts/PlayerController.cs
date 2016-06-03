using UnityEngine;
using System.Collections;

/// <summary>
/// the vr controller script
/// </summary>
public class PlayerController : MonoBehaviour
{
    //currently held object on this controller if set to null show the vive controller
    private Item _item;
    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            showViveModel(value == null);
        }
    }

    //refrence to the model of the controller
    public GameObject viveModel;

    //VR VARS
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;  // Buttons on controller references and booleans for when they are and are not being pressed
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    
    // Haptic feedback
    public float hapticIntensity = 1500f;
    public float hapticTime = 0.1f;
    public float hapticStrength = 0.1f;

    // clicked variables
    public bool isTriggerClicked = false;
    public bool isGripClicked = false;

    public bool hasItem
    {
        get { return item == null ? false : true; }
    }
    
	void Start () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        item = null;
        if (trackedObject == null)
        {
            enabled = false;
        }
    }
	
	void Update ()
    {
        // check to see if the controller is null, and not being detected
        if (controller == null) { return; }
        Debug.LogWarning(hasItem);
        //TRIGGER
        if(controller.GetPressDown(triggerButton))
        {
            isTriggerClicked = true;
            if (item != null)
            {
                if (item.type == Hydrogen.ItemType.Gun) {
                    if (((Gun)item).shoot())
                    {
                        RumbleController(hapticTime, hapticStrength);
                    }
                }
            }
        }
        if (controller.GetPress(triggerButton))
        {
            isTriggerClicked = true;
        }
        if (controller.GetPressUp(triggerButton))
        {
            isTriggerClicked = false;
        }


        //GRIP
        if (controller.GetPressDown(gripButton))
        {
            isGripClicked = true;
            if (item != null)
            {
                if (item.type == Hydrogen.ItemType.Gun)
                {
                    ((Gun)item).dropMagazine();
                }
            }
        }
        if (controller.GetPress(gripButton))
        {
            isGripClicked = true;
        }
        if (controller.GetPressUp(gripButton))
        {
            isGripClicked = false;
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
    
    public void showViveModel(bool hide)
    {
        viveModel.SetActive(hide);
    }
}
