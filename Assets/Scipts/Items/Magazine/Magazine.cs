using UnityEngine;
using NewtonVR;
using Hydrogen;
public abstract class Magazine : NVRInteractableItem
{
    public MagazineType magazineType;
    public Bullet bullet;
    private int _currentBulletCount;
    private int _maxBulletCount;
    private bool _isEquipped;

    protected override void Start()
    {
        base.Start();
        _maxBulletCount = 10;
        _currentBulletCount = _maxBulletCount;
    }

    public int bulletCount
    {
        get { return _currentBulletCount; }
        set { _currentBulletCount = Mathf.Clamp(value, 0, _maxBulletCount); }
    }

    public int maxBulletCount
    {
        get { return _maxBulletCount; }
        set { _maxBulletCount = value; }
    }
    
    public bool isEquipped
    {
        get { return _isEquipped; }
        set { _isEquipped = value; }
    }

    public bool hasBullets
    {
        get { return _currentBulletCount > 0; }
    }
}