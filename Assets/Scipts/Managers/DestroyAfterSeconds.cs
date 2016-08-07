using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// this is a basic utility class used for destrying any Gameobject
    /// this script is attached to
    /// </summary>
   
    public class DestroyAfterSeconds : MonoBehaviour
    {
        public float secondsToDestroy = 0f;
        private float _timeSenseSpawn = 0f;

        void Update()
        {
            _timeSenseSpawn += Time.deltaTime;

            if (_timeSenseSpawn >= secondsToDestroy)
                Destroy(gameObject);
        }
    }
}
