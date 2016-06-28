using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour , Projectile
{
    //public BulletType bulletType;
    public float bulletForce = 100.0f;
    private float _penetration;

    private float _secondsUntilDestroy = 5.0f;

    //All bullets need to add there own force
    public void initialize()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce, ForceMode.Impulse);
        _penetration = 0;
        if (gameObject.activeSelf)
        {
            StartCoroutine(destroyBullet(_secondsUntilDestroy));
        }
    }

    //To make a bullet able to destroy more than one target
    public void initialize(float penetration)
    {
        initialize();
        _penetration = penetration;
    }


    public float penetration
    {
        set
        {
            _penetration -= value;
            if (_penetration < 0) Destroy(gameObject);
        }
    }

    public Vector3 position
    {
        get
        {
            return transform.position;
        }
    }


    void OnTriggerEnter(Collider hit)
    {
        //Can't test this myself, but its simple and should work.
        switch(hit.gameObject.tag)
        {
            //all tags have caps for first letter
            case "Wall":
                gameObject.SetActive(false);
                break;
        }
    }

    IEnumerator destroyBullet(float _numSeconds)
    {
        yield return new WaitForSeconds(_numSeconds);
        Destroy(gameObject);
    }
}
