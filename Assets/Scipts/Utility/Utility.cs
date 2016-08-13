using UnityEngine;
using System.Collections.Generic;

namespace Hydrogen
{
    /// <summary>
    /// This class is for storing enums and int/float constants were 
    /// we can find them easily in the Hydrogen namespace
    /// </summary>
    public static class GameConstants
    {
        public const string MAGAZINE = "Magazine";

        public enum Difficulty
        {
            Easy,
            Normal,
            Hard,
            Extreme
        }

        // Any weapon in the game can be categorized into one of these weapons
        public enum WeaponType
        {
            Automatic, AutomaticEngage, Semiautomatic, SemiautomaticEngage, Repeater, Charge
        }


        // this is going to be a static dictionary that needs to be accessble to Gun class so  it can init flag values easily
        public static Dictionary<WeaponType, GunTypeInitValues> gunTypeInitValues = new Dictionary<WeaponType, GunTypeInitValues>()
        {
            { WeaponType.Automatic, new GunTypeInitValues(_isAutomatic:true)},
            { WeaponType.AutomaticEngage, new GunTypeInitValues(_isAutomatic:true, _needsEnagement:true)},
            { WeaponType.SemiautomaticEngage, new GunTypeInitValues(_needsEnagement:true)},
            { WeaponType.Semiautomatic, new GunTypeInitValues()},
            { WeaponType.Charge, new GunTypeInitValues( _isCharge:true)},
            { WeaponType.Repeater, new GunTypeInitValues( _isRepeater:true, _needsEnagement:true)}
        };

        public enum TargetPart { Inner, Middle, Outer };
    }

    public static class Utility
    {
        // This is a basic method for getting a Component on GameObject using a tag by default and name if specified
        public static void InitGameObjectComponent<T>(string objectStringID, out T gameObject, bool byName = false)
            where T : MonoBehaviour
        {
            gameObject = null;

            if (byName)
            {
                GameObject foundGameObject = GameObject.Find(objectStringID);
                if (foundGameObject == null)
                {
                    Debug.LogError(string.Format("GameObject with name {0} not found...", objectStringID));
                    return;
                }
                gameObject = foundGameObject.GetComponent<T>();
                //Log error if not found
                if (gameObject == null)
                    Debug.LogError(string.Format("The component for GameObject {0} was not found...", objectStringID));
            }
            else
            {
                GameObject foundGameObject = GameObject.FindGameObjectWithTag(objectStringID);
                if (foundGameObject == null){
                    Debug.LogError(string.Format("GameObject with name {0} not found...", objectStringID));
                    return;
                }

                gameObject = foundGameObject.GetComponent<T>();
                //Log error if not found
                if (gameObject == null)
                    Debug.LogError(string.Format("The component for GameObject {0} was not found...", objectStringID));
            }    
        }
    }
}
