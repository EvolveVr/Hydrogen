using UnityEngine;
using NewtonVR;
using System.Collections.Generic;
using Hydrogen;

[RequireComponent(typeof(Rigidbody))]
public abstract class Gun : NVRInteractableItem
{
    //This gun type
    public GunType gunType;
    //current magazine that's in it
    public Magazine currentGunMagazine;
    //the minimum damage the gun will do
    private float _baseDamage = 10f;
    // how much force we're applying to the bullet
    public float _bulletForce = 300.0f;
    // force applied to the bullet casing object after fire
    private float _casingForce = 50.0f;
    // does the gun have an equipped magazine
    public bool isLoaded
    {
        get { return currentGunMagazine != null; }
    }

    #region COMPONENT REFRENCES
    public Transform firePosition;
    public Transform casingPosition;
    public Transform magazinePosition;
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
    
    public override void UseButtonDown()
    {
        base.UseButtonDown();
        shoot();
    }

    public void shoot()
    {
        if (!isLoaded) { Debug.Log("DOESN'T HAVE A MAGAZINE. NOT GOING TO SHOOT"); return; }
        if (currentGunMagazine.hasBullets)
        {
            Debug.Log("Shoot");
        }
    }
    
    protected override void Update()
    {
        base.Update();  
    }

    // for checking if magazine is being put into thr collider
    void OnTriggerEnter(Collider other)
    {

    }
}