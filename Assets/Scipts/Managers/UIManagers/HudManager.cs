using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Hydrogen;
public class HudManager : MonoBehaviour
{
    public GameObject gunUI;
    public Text MyGunText;
    public Gun myGun;

    void Start()
    {
        gunUI.SetActive(false);
    }

    public void Update()
    {
        if (myGun.IsAttached)
        {
            if (!gunUI.activeInHierarchy)
            {
                gunUI.SetActive(true);
            }
            setText();

        }
        else
        {
            if (gunUI.activeInHierarchy)
            {
                gunUI.SetActive(false);
            }
        }
    }

    public void setText()
    {
        MyGunText.text = myGun.weaponName + "\nLOADED" + myGun.isLoaded.ToString() + "\nENGAGED" + myGun.isEngaged.ToString();
        if (myGun.isLoaded)
        {
            MyGunText.text += "\nCurrentBullets: " + myGun.currentMagazine.currentBulletCount + "/" + myGun.currentMagazine.maxBulletCount;
        }
        else
        {
            MyGunText.text += "";
        }
    }
}
