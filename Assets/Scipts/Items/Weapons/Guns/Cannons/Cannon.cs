using UnityEngine;
using Hydrogen;
using System.Collections;
using NewtonVR;

/// <summary>
/// This will eventually be turned into an abstract class;
/// This repreesents all guns that the user must insert the projectile
/// into the weapon and then it is automatically engaged
/// </summary>

public class Cannon : Gun
{
    public GameObject ammoPrefab;
    private Rigidbody rbOfEngagedBullet;

    //Properties below
    //Checks if object is within cannon's trigger;
    void OnTriggerEnter(Collider hit)
    {
        //if it is the proper bullet type
        if (hit.gameObject.tag == "CannonBullet")
        {
            //1) transport and 2) set rotation and 3) set it to is kinematic and set parent = fire point; 

            hit.gameObject.transform.position = firePoint.position;
            hit.gameObject.transform.rotation = firePoint.rotation;

            rbOfEngagedBullet = hit.gameObject.GetComponent<Rigidbody>();

            rbOfEngagedBullet.isKinematic = true;
            rbOfEngagedBullet.useGravity = false;
            rbOfEngagedBullet.gameObject.transform.SetParent(firePoint);

            isEngaged = true;
        }
    }

    //Methods below

    public override void UseButtonDown()
    {
        base.UseButtonDown();

        if (isEngaged)
        {
            shootGun();
        }
    }

    protected override void shootGun()
    {
        //before adding the force, you you have a few things to do:
        // 1) set isKinematic to false and 2) set the gravity on the rigidbody
        rbOfEngagedBullet.isKinematic = false;
        rbOfEngagedBullet.useGravity = true;

        Bullet bullet = rbOfEngagedBullet.gameObject.GetComponent<Bullet>();


        //this should not be null but this is just in case;
        if (bullet)
        {
            bullet.addForce();
        }
        else
        {
            Debug.Log("Warning!!!!! (My own Warning): bullet was somehow null in the shootGun method of Cannon. Not good! Fix");
        }

        isEngaged = false;
    }
    
}
