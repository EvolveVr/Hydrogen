using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public GameObject bulletPreFab;
    public float bulletForce = 100.0f;

    public void addForce()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce, ForceMode.Impulse);
    }
    void OnTriggerEnter(Collider hit)
    {
        //Can't test this myself, but its simple and should work.
        switch(hit.gameObject.tag)
        {
            case "wall":
                gameObject.SetActive(false);
                break;

        }

    }
}
