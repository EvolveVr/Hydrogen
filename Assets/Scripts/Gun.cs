using UnityEngine;
using System.Collections;
/// <summary>
/// This class deals with all the gun methods and specific gun variables
/// </summary>
public class Gun : Item
{
    //gun variables
    private int _baseDamage;
    public Magazine equippedMagazine;
    public int damage { get { return baseDamage + equippedMagazine.bullet.damage; } }   //the total amount of damage will be the baseDamage + the bulletDamage
    
    //gun refrences
    public GameObject magazinePrefab;
    public Transform magazineParent;

    #region GETTERS AND SETTERS
    public int baseDamage
    {
        get { return _baseDamage; }
        set { _baseDamage = value; }
    }

    public bool hasMagazine
    {
        get { return equippedMagazine == null ? false : true; }
    }
    #endregion

    #region CONSTRUCTORS
    //empty constructor leave it
    public Gun() {
        type = Hydrogen.ItemType.Gun;
    }

    public Gun(string Name, int Weight) : base(Name, Weight) {

        type = Hydrogen.ItemType.Gun;
    }

    public Gun(string Name, int Weight, int BaseDamage) : base(Name, Weight)
    {
        type = Hydrogen.ItemType.Gun;
        baseDamage = BaseDamage;
    }
    #endregion

    #region GUN METHODS
    public virtual void reload()
    {
        
    }

    public virtual bool shoot()
    {
        if (hasMagazine)
        {
            if (equippedMagazine.bulletCount > 0)
            {
                equippedMagazine.MakeBullet();
                return true;
            }
            return false;
        }
        else
        {
            Debug.Log("YOU DON'T HAVE A MAGAZINE");
            return false;
        }
    }

    public virtual void equip(Magazine mag)
    {
        equippedMagazine = mag;

        equippedMagazine.equipMagazine(true);

        //un parent magazine
        mag.transform.SetParent(magazineParent);
        
        //turn off physics for magazine
        mag.GetComponent<Rigidbody>().isKinematic = true;
        mag.GetComponent<Rigidbody>().useGravity = false;

        //change position
        mag.transform.position = magazineParent.position;
        mag.transform.rotation = magazineParent.rotation;
    }

    public virtual void dropMagazine()
    {
        //un parent the magazine
        equippedMagazine.gameObject.transform.SetParent(null);
        //turn on gravity for the magazine
        equippedMagazine.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        equippedMagazine.gameObject.GetComponent<Rigidbody>().useGravity = true;

        equippedMagazine.equipMagazine(false);
        equippedMagazine = null;
    }
    #endregion

    

    void Start()
    {
        equip(equippedMagazine);
    }

    public override void Update()
    {
        if (!equipped)
        {
            Debug.LogWarning("CALLING BASE UPDATE - Listening for player controller to pick this gun up");
            base.Update();
        }
        else
        {
            
        }
    }
}