using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Hydrogen
{
 /// <summary>
 /// This class is used as a basic Gun manager for the Gun canvas where users will be choosing 
 /// weapons. This will be finding all the guns in the scene and populate the UI for players to choose
 /// and activate the corresponding gun they choose.
 /// Here, we will use the Singleton programming paradigm because we only need one instance at any time
 /// </summary>
 ///
    public class WeaponUIManager : MonoBehaviour
    {
        //Singleton
        private static WeaponUIManager _weaponUIManager = null;

        private WeaponPanel[] _weaponPanels;
        private Weapon[] _weapons;

        #region Public Properties

        //This is used to return the single instance weaponUImanager in the scene
        public static WeaponUIManager Instance
        {
            get
            {
                return _weaponUIManager;
            }
        }

        #endregion

        #region Unity Functions
        void Awake()
        {
            //before doing anything, we need a WeaponUIManager instance; we only want one instance
            if (_weaponUIManager == null)
                _weaponUIManager = this;
            else if (_weaponUIManager != null)
                Destroy(gameObject);


            _weaponPanels = GameObject.FindObjectsOfType<WeaponPanel>();
            _weapons = GameObject.FindObjectsOfType<Weapon>();

            initWeaponPanelInfo();
            deactivateWeaponsOnStart();
        } //end awake()
        #endregion

        #region Private Methods
        //Here we check the if panels and weapons are null and if not
        // then go ahead and assign the values from the weapon to the
        // gun panel for viewing on UI
        private void initWeaponPanelInfo()
        {
            if (_weaponPanels != null && _weapons != null)
            {
                for (int i = 0; i < _weaponPanels.Length; i++)
                {
                    Image myGunImage = _weapons[i].GetComponent<Image>();
                    _weaponPanels[i].gunImage.sprite = myGunImage.sprite;
                    _weaponPanels[i].gunTitle.text = _weapons[i].weaponName;
                    _weaponPanels[i].gunDescription.text = _weapons[i].weaponDescription;

                    //now assign the weapon to the panel so that we can activate it on button press
                    _weaponPanels[i].assignedWeapon = _weapons[i];
                }
            }
        }

        // Deactivate all weapons after all references are collected
       private void deactivateWeaponsOnStart()
       {
            if(_weapons != null)
            {
                foreach(Weapon weapon in _weapons)
                {
                    weapon.gameObject.SetActive(false);
                }
            }
       }
        #endregion

        #region Public Methods
        //this function is meant to deactivate all button when the button is pressed
        public void setButtonInactive()
        {
            if (_weaponPanels != null)
            {
                foreach (var panel in _weaponPanels)
                {
                    panel.chooseWeaponButton.interactable = false;
                }
            }
        }
        #endregion

    }
}
