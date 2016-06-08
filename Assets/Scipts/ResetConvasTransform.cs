using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResetConvasTransform : MonoBehaviour
{
    public float rotationX = 0.0f;
    public float rotationY = 0.0f;
    public float rotationZ = 0.0f;

    public Text text;
    private GameObject controllerLeft;
	// Use this for initialization
	void Start ()
    {
        text.enabled = true;
        controllerLeft = GameObject.FindGameObjectWithTag("ControllerLeft");

        if (controllerLeft != null)
        {
            this.gameObject.transform.position = controllerLeft.transform.position;
            gameObject.transform.localPosition = new Vector3(-0.09521663f, 4.286187e-05f, -1.678709e-07f);
            gameObject.transform.rotation = Quaternion.Euler(0,0,0);

            Debug.Log("Left Controller set");
        }

	}
}
