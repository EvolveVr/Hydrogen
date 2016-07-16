using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    Transform player;
    void Awake()
    {
        player = GameObject.Find("player").GetComponent<Transform>();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(player);
	}
}
