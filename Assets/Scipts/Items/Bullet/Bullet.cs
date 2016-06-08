using UnityEngine;
using System.Collections;
using Hydrogen;

public abstract class Bullet : MonoBehaviour
{
    public BulletVariation variation;
    private int _bulletDamage;
    public int bulletDamage
    {
        get { return _bulletDamage; }
        set { _bulletDamage = value; }
    }
}