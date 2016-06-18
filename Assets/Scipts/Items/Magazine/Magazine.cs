using UnityEngine;
using NewtonVR;
using System.Collections;

//isEquipped from the Magazines perspective means the Magazine is inside a gun
namespace Hydrogen
{
    public class Magazine : NVRInteractableItem
    {
        public GunType magazineType;
        public Bullet bullet;
        public Transform interactionPoint;
        private int _currentBulletCount;
        private int _maxBulletCount;
        private bool _isEquipped;
        public Gun myGun;

        public float secondsAfterDetach = 0.1f;

        protected override void Start()
        {
            base.Start();
            if (myGun != null)
            {
                _isEquipped = true;
            }
            _maxBulletCount = 15;
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

        public GameObject getBullet
        {
            get
            {
                _currentBulletCount--;
                return Instantiate<GameObject>(bullet.gameObject);
            }
        }

        public void attachMagazine(bool attach, GameObject gun = null)
        {
            if (attach)
            {
                myGun = gun.GetComponent<Gun>();
                if (myGun.isLoaded || !myGun.IsAttached) { Debug.Log("I ALREADY HAVE A MAGAZINE IN ME"); return; }

                if (magazineType == myGun.GunType)
                {
                    isEquipped = true;
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

        // This is for the reload animation
        protected void reloadGunAnim() { }

        public IEnumerator enableCollider()
        {
            yield return new WaitForSeconds(secondsAfterDetach);
            GetComponent<BoxCollider>().isTrigger = false;
            CanAttach = true;
            StopCoroutine(enableCollider());
        }
    }
}