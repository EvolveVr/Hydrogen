using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hydrogen;
public class BulletManager : MonoBehaviour
{
    
    //We'll be passing in the derived classes of gun as keys, then the bullet pools as values
    Dictionary<Gun, List<GameObject>> allBulletPools;

	void Awake()
    {
        
        allBulletPools = new Dictionary<Gun, List<GameObject>>();  
    }
	void Start ()
    {
     //We'll add in the other types of bulletPools in here 
       allBulletPools.Add(GameObject.Find("PM-40").GetComponent<SemiAutomaticGun>(),gameObject.GetComponent<HandGunBulletPool>().getPool);

        //This was testing to make sure different derived gun types worked as keys and they did.
   //  allBulletPools.Add(GameObject.Find("tempgun").GetComponent<AutomaticGun>(), gameObject.GetComponent<HandGunBulletPool>().getPool);
	}
	
	public GameObject chooseBullets(int bulletsShot, Gun currentBullets)
    {
        //number of bulletsShot will increment before calling this function, so doing decrementing it since arrays start at 0

        //Returns the bulletPool for right gun

        
        return allBulletPools[currentBullets][bulletsShot-1];

    }
	
}
