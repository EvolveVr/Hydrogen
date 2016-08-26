using UnityEngine;
using UnityEngine.UI;
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

        // modifying mathf.pingpong so I can get negative values as well
        public class PingPong
        {
            float _limit = 1f;
            bool justChanged = false;
            int modValue = 0;

            public PingPong(float limit)
            {
                _limit = limit;
            }

            // we pass in Time.time because its a constantly changing value
            public float PingPongMod(float time)
            {
                float value = Mathf.PingPong(time, _limit);
                if (Mathf.Abs(value) <= 0.1f && !justChanged) {
                    modValue++;
                    justChanged = true;
                }
                else if(Mathf.Abs(value) <= 0.1f && justChanged)
                {
                    if(value < 0) {
                        value -= 0.15f;
                    }
                    else {
                        value += 0.15f;
                    }

                    justChanged = false;
                }


                if(modValue % 2 == 0)
                {
                    return value;
                }
                else
                {
                    return -value;
                }
            }
        }

		// Basic Utility method for Debugging messages More quickly
		// instead of constantly having to write if statements;
		// this will normally be preceeded by statement for finding objects
		public static void DubugMessage<T>(T someObj)
		{
			if (someObj != null) {
				Debug.Log ("Object WAS found");
			} else {
				Debug.Log ("Object Was NOT found");
			}
		}
    }

}
