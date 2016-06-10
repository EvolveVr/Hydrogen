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
}
