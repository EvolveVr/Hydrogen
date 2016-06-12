using UnityEngine;
using NewtonVR;
using Hydrogen;
using System.Collections;

//isEquipped from the Magazines perspective means the Magazine is inside a gun

public class Magazine : NVRInteractableItem
{
    public MagazineType magazineType;
    public Bullet bullet;
    BulletManager bulletPools;
    string currentBulletPool;
    public Transform interactionPoint;
    private int _currentBulletCount;
    private int _maxBulletCount;
    private bool _isEquipped;
    public Gun myGun;
    public float secondsAfterDetach = 0.1f;
    public Magazine()
    {
        //Put it in constructor, because initializing pool is also in awake and got bullet count before this was initialized to 10.
        //Left like for testing purposes, im guessing we could inherit magazines so that we can construct number of bullets for each type of magazine.
        _maxBulletCount = 10;

        //I made it zero because it's going to increment with every bulletshot until it reaches max, changed this so we could use currentbullet count to iterate through the bullet pool.
        _currentBulletCount = 0;
    
    }
 
     void Start()
    {
        base.Start();
        if(myGun != null)
        {
            _isEquipped = true;
        }
         bulletPools = GameObject.Find("GameManager").GetComponent<BulletManager>();
      
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
        get { return _currentBulletCount <= _maxBulletCount; }
    }

    public GameObject getBullet
    {
        get
        {
            _currentBulletCount++;
            return bulletPools.chooseBullets(_currentBulletCount, myGun);
        }
    }

    public void attachMagazine(bool attach, GameObject gun = null)
    {
        if (attach)
        {
            isEquipped = true;
            myGun = gun.GetComponent<Gun>();
            GetComponent<BoxCollider>().isTrigger = true;
            transform.SetParent(myGun.magazinePosition, false);
            CanAttach = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            transform.position = myGun.magazinePosition.transform.position;
            transform.rotation = myGun.magazinePosition.transform.rotation;
            transform.localScale = Vector3.one;
            myGun._currentMagazine = this;
        }
        else
        {
            transform.SetParent(null);
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            isEquipped = false;
            StartCoroutine(enableCollider());
        }
    }

    public IEnumerator enableCollider()
    {
        yield return new WaitForSeconds(secondsAfterDetach);
        GetComponent<BoxCollider>().isTrigger = false;
        CanAttach = true;
        StopCoroutine(enableCollider());
    }
    
}