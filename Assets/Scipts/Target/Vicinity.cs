using UnityEngine;
using System.Collections;
/// <summary>
/// This is just to make sure targets remain within vicinity of the anchor
/// Might do more with this like change size of it more targets killed or something like that.
/// </summary>
public class Vicinity : MonoBehaviour {
    private Transform _vicinityOf;
	// Use this for initialization
    void Awake()
    {
        _vicinityOf = transform.parent.GetComponent<Transform>();
    }
	private void OnTriggerExit(Collider left)
    {
        if (left.tag == "Target")
        {//When target leaves vicinity move it towards anchor by a tiny amount and let the movement reversal on target script do rest.
            left.transform.position = Vector3.MoveTowards(left.transform.position, left.transform.parent.position, Time.fixedDeltaTime * 2);
        }
    }
}
