using UnityEngine;
using System.Collections;

public class Magazine : Item
{
    private int _bulletCap;     //Max amount of bullets
    private int _bulletCount;   //Current bullet count
    public Bullet bullet;
    public GameObject bulletPrefab;
    public Transform shootingPoint;

    #region GETTERS AND SETTERS
    public bool hasAmmo { get { return bulletCount > 0 ? true : false; } }

    public int bulletCount
    {
        get { return _bulletCount; }
        set { _bulletCount = value; }
    }

    public int bulletCap
    {
        get { return _bulletCap; }
        set { _bulletCap = value; }
    }
    #endregion

    void Awake()
    {
        _bulletCap = 12;
        _bulletCount = _bulletCap;
    }

    public override void pickUp(Transform parentToChild)
    {
        if (!equipped)
        {
            base.pickUp(parentToChild);
        }
    }

    public void MakeBullet()
    {
        if(bulletCount > 0)
        {
            Debug.LogWarning("USING BULLET NUMBER: " + bulletCount);
            bulletCount--;
            GameObject temp = (GameObject)Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        }
        else
        {
          Debug.Log("MAGAZINE IS EMPTY");
        }
    }

    public void equipMagazine(bool equip)
    {
        if (equip)
        {
            equipped = true;
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            equipped = false;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}