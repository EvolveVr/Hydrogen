using UnityEngine;
using System.Collections;

public class Magazine : Item
{
    private int _bulletCap;     //Max amount of bullets
    private int _bulletCount;   //Current bullet count
    public Bullet bullet;

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
}