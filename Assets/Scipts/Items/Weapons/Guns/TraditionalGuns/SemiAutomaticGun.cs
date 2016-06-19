using UnityEngine;
using Hydrogen;
using System.Collections;

public class SemiAutomaticGun : Gun
{

    public Magazine _currentMagazine;
    public Transform magazinePosition;

    public float secondsAfterDetach = 0.2f;

    //Properties

    public bool isLoaded
    {
        get { return _currentMagazine != null; }
    }

    //Methods below

    protected override void shootGun()
    {
        if (isLoaded)
        {
            if (_currentMagazine.hasBullets && isEngaged)
            {
                GameObject bullet = _currentMagazine.getBullet;
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;
                StartCoroutine(LongVibration(VibrationCount, VibrationLength, gapLength, VibrationStrength));
                StopCoroutine(LongVibration(VibrationCount, VibrationLength, gapLength, VibrationStrength));
                bullet.GetComponent<Bullet>().addForce();
            }
        }
    }

    //not all guns have a magazine
    protected void dropCurrentMagazine()
    {
        if (isLoaded)
        {
            StartCoroutine(disableMagazineCollider());
            _currentMagazine.attachMagazine(false);
            _currentMagazine = null;

        }
        _isEngaged = false;
    }

    public override void UseButtonDown()
    {
        shootGun();
    }

    protected override void Update()
    {
        base.Update();
        if (AttachedHand != null)
        {
            if (AttachedHand.Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                dropCurrentMagazine();
            }
        }
    }

    public IEnumerator disableMagazineCollider()
    {
        magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(secondsAfterDetach);
        magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = true;
        StopCoroutine(disableMagazineCollider());
    }

    //unimplemented
    protected void applySpreadToBullet() { }

    protected void updateEngageLevel() { }

    protected void playEngageSound() { }

    protected void playNonEngagedSound() { }

    protected float getControllerTriggerPosition() { return 0.0f; }

    protected void updateTriggerPosition() { }

    public void updateEngageLeverPosition() { }
}
