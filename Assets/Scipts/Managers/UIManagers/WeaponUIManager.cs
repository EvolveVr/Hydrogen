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
        private Weapon[] _weapons;
        private WeaponPanel[] _weaponPanels;

        void Awake()
        {
            _weapons = GameObject.FindObjectsOfType<Weapon>();

            if(_weapons != null)
            {
                foreach(var weapon in _weapons)
                {
                    Debug.Log(weapon.name);
                }
            }

            _weaponPanels = GameObject.FindObjectsOfType<WeaponPanel>();

            if (_weaponPanels != null)
            {
                
            }
        }

    }
}
