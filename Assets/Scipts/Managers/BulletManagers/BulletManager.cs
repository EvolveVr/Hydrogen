using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hydrogen;


public class BulletManager : MonoBehaviour
{
    //We'll be passing in the derived classes of gun as keys, then the bullet pools as values
    Dictionary<Weapon, List<GameObject>> allBulletPools;

	void Awake()
    {
        allBulletPools = new Dictionary<Weapon, List<GameObject>>();  
    }

	public GameObject chooseBullets(int bulletsShot, Weapon currentBullets)
    {
        return allBulletPools[currentBullets][bulletsShot-1];
    }
	
}
