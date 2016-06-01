using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private int _damage; //the damage that will be added into the total gun damage
    public int damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public Rigidbody rb;
    public float speed = 800f;

    public GameObject ParentGun;
    public GameObject Controller;

    public bool add = false;

    public float timeToDestroy = 5f;
    
    public void Start()
    {
        add = true;
    }

    void Update()
    {
        if (add)
        {
            if(timeToDestroy < 0)
            {
                hit();
            }
            else
            {
                rb.AddForce(transform.up * speed);
                timeToDestroy -= Time.deltaTime;
            }
        }
    }

    public void hit()
    {
        add = false;
        //ParticleEffect
        Destroy(this.gameObject, 0.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other.name);
        if (other.name != "GunParent")
        { hit(); }
    }
}