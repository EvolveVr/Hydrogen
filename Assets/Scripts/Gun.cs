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

    #region GETTERS AND SETTERS
    public int baseDamage
    {
        get { return _baseDamage; }
        set { _baseDamage = value; }
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
        if (currentMagazine.hasAmmo)
        {
            currentMagazine.bulletCount--;
        }
        else
        {
            Debug.Log("DOESN'T HAVE AMMO!!! SWITCH MAGAZINE AND RELOAD!");
        }
    }
    #endregion
}