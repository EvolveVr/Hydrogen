using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// this class is used for making getting references to its children to make it easier 
    /// to get references for the WeaponUIManager
    /// </summary>
    public class WeaponPanel : MonoBehaviour
    {
        #region Public Variables
        public Image gunImage;
        public Text gunTitle;
        public Text gunDescription;
        #endregion

        #region Private Variables
        private Button _chooseWeaponButton;
        private Weapon _assignedWeapon;
        #endregion

        #region Properties
        public Weapon assignedWeapon
        {
            get { return _assignedWeapon; }

            set { _assignedWeapon = value; }
        }

        public Button chooseWeaponButton
        {
            get { return _chooseWeaponButton; }
        }
        #endregion


        #region Unity Functions
        void Awake()
        {
            _chooseWeaponButton = GetComponentInChildren<Button>();

            if (_chooseWeaponButton == null)
                Debug.LogError("The button component on the gun panel was not found; this is an error.");
        }
        #endregion

        // When the button on the weapon panel is choosen, then set the corresponding weapon active
        public void setWeaponActive()
        {
            _assignedWeapon.gameObject.SetActive(true);
            WeaponUIManager.Instance.setButtonInactive();
        }

        
    }
}
