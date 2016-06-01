using UnityEngine;
using System.Collections;
/// <summary>
/// This class deals with all the gun methods and specific gun variables
/// </summary>
public class Gun : Item
{
    private int _baseDamage;     //the total amount of damage will be the baseDamage + the bulletDamage
    public int damage { get { return baseDamage + currentMagazine.bullet.damage; } }
    public Magazine currentMagazine;
    public GameObject magazinePrefab;
    public Transform magazineParent;

    #region GETTERS AND SETTERS
    public int baseDamage
    {
        get { return _baseDamage; }
        set { _baseDamage = value; }
    }

    public bool isLoaded
    {
        get { return currentMagazine == null ? false : true; }
    }
    #endregion

    #region CONSTRUCTORS
    //empty constructor leave it
    public Gun() { }

    public Gun(string Name, int Weight) : base(Name, Weight) { }

    public Gun(string Name, int Weight, int BaseDamage) : base(Name, Weight)
    {
        baseDamage = BaseDamage;
    }
    #endregion

    #region GUN METHODS
    public virtual void reload()
    {
        /*
        TODO: REMOVE MAGAZINE ADD IN ANOTHER
        */
    }

    public virtual void shoot()
    {
        currentMagazine.MakeBullet();
    }

    public virtual void equip(Magazine mag)
    {
        currentMagazine = mag;
        mag.equipped = true;
        mag.transform.SetParent(magazineParent);
        mag.GetComponent<Rigidbody>().isKinematic = true;
        mag.GetComponent<Rigidbody>().useGravity = false;
        currentMagazine.gameObject.GetComponent<BoxCollider>().enabled = false;
        mag.transform.position = magazineParent.position;
        mag.transform.rotation = magazineParent.rotation;
    }

    public virtual void dropMagazine()
    {
        currentMagazine.gameObject.transform.SetParent(null);
        currentMagazine.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        currentMagazine.gameObject.GetComponent<Rigidbody>().useGravity = true;
        currentMagazine.gameObject.GetComponent<BoxCollider>().enabled = true;
        currentMagazine.equipped = false;
    }
    #endregion

    void Start()
    {
        equip(currentMagazine);
    }

    public override void Update()
    {
        base.Update();
        if (!equipped) { return; }
        if(myCollidingObj==null) { return; }
        if (myCollidingObj.GetComponent<PlayerController>() != null)
        {
            if (myCollidingObj.GetComponentInChildren<Magazine>() != null)
            {
                Magazine myMag = myCollidingObj.GetComponentInChildren<Magazine>();
                if (!isLoaded)
                {
                    equip(myMag);
                }
            }

            /*
            TODO: FOR NOW, ITEMS WILL BE COLLECTED AS SOON AS YOU RUN INTO THEM
            WE NEED TO MAKE IT SO WHEN TRIGGER CLICKED AND ITEM IS CURRENTLY COLLIDING - THEN PICK UP
            VIBRATE CONTROLLER
            */
        }
    }
}