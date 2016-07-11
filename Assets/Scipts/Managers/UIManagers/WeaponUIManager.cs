using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Hydrogen
{
 /// <summary>
 /// This class is used as a basic Gun manager for the Gun canvas where users will be choosing 
 /// weapons. This will be finding all the guns in the scene and populate the UI for players to choose
 /// and activate the corresponding gun they choose.
 /// </summary>
 ///
    public class WeaponUIManager : MonoBehaviour
    {
        private WeaponPanel[] _weaponPanels;
        private Weapon[] _weapons;

        void Awake()
        {
            _weaponPanels = GameObject.FindObjectsOfType<WeaponPanel>();
            _weapons = GameObject.FindObjectsOfType<Weapon>();

            if (_weaponPanels != null && _weapons != null)
            {
                for (int i = 0; i < _weaponPanels.Length; i++)
                {
                    Image myGunImage = _weapons[i].GetComponent<Image>();
                    _weaponPanels[i].gunImage.sprite = myGunImage.sprite;
                    _weaponPanels[i].gunTitle.text = _weapons[i].weaponName;
                }
            }
        }
    }
}
