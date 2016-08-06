using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// this class is used for detecting collisions on the inner part of the target
    /// </summary>
    public class InnerTarget : MonoBehaviour
    {
        private Target _parentTarget;

        public int _numberOfPointsToAdd;

        void Awake()
        {
            _parentTarget = GetComponentInParent<Target>();
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Logging");
            if(other.tag == "Bullet")
            {
                GameManager.gameManager.AddPoints(_numberOfPointsToAdd);
                Debug.Log(GameManager.gameManager.PlayersPoints);

                //spawn particle effect at correct position; set parent to null
                _parentTarget.InitParticleEffect(GameConstants.TargetPart.Inner);

                //play sound for target

                Destroy(transform.parent.gameObject);
            }
        }
    }
}
