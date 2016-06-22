using UnityEngine;
using NewtonVR;
using System.Collections;

//isEquipped from the Magazines perspective means the Magazine is inside a gun
namespace Hydrogen
{
    public class Magazine : NVRInteractableItem
    {
        public Transform interactionPoint;
        
        //the correct corresponding bullet to this magazine
        public Bullet bullet;
        
        //can the gun shoot continuously or not
        public bool hasBulletCount = false;

        //the current number of bullets inside the magazine
        public int currentBulletCount;

        //the max amount of bullets that fit inside that magazine
        public int maxBulletCount;

        //is the magazine currently in a gun
        public bool isEquipped;

        //the gun this magazine is equipped to
        public Gun myGun;

        //the seconds after detatching to turn on the magazine collider
        public float secondsAfterDetach = 0.1f;
        
        #region PROPERTIES
        public int bulletCount
        {
            get { return currentBulletCount; }
            set { currentBulletCount = Mathf.Clamp(value, 0, maxBulletCount); }
        }
        
        public bool hasBullets
        {
            get { return currentBulletCount > 0; }
        }

        public GameObject getBullet
        {
            get
            {
                if (hasBulletCount) { bulletCount--; }
                return Instantiate(bullet.gameObject);
            }
        }
        #endregion

        protected override void Start()
        {
            base.Start();
            if (myGun != null)
            {
                isEquipped = true;
            }
            currentBulletCount = maxBulletCount;
        }

        #region MAGAZINE METHODS
        public void attachMagazine(bool attach, GameObject gun = null)
        {
            if (hasBulletCount)
            {
                if (attach)
                {
                    myGun = gun.GetComponent<Gun>();
                    if (myGun.isLoaded || !myGun.IsAttached) { return; }

                    //TODO: ADD IN A CHECK TO SEE IF IT'S THE PROPER MAGAZINE FOR THE CURRENT GUN                
                    isEquipped = true;
                    GetComponent<BoxCollider>().isTrigger = true;
                    transform.SetParent(myGun.magazinePosition, false);
                    CanAttach = false;
                    GetComponent<Rigidbody>().isKinematic = true;
                    GetComponent<Rigidbody>().useGravity = false;
                    transform.position = myGun.magazinePosition.transform.position;
                    transform.rotation = myGun.magazinePosition.transform.rotation;
                    transform.localScale = Vector3.one;
                    myGun.currentMagazine = this;

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
        #endregion
    }
}