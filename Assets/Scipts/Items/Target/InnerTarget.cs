using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// Basic class for adding points to Target Parent
    /// </summary>
    public class InnerTarget : MonoBehaviour
    {
        public GameConstants.TargetPart targetPart;
        private Target _parentTarget;
        public int pointsToGive = 0;

        void Awake()
        {
            _parentTarget = GetComponentInParent<Target>();
            if (_parentTarget == null)
                Debug.LogError("Could not find parent target");
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Bullet")
            {
                //add points --> init particl effect --> play audio --> destroy
                _parentTarget.AddPoints(pointsToGive);
                _parentTarget.InitParticleEffect(targetPart, other.transform.position);
                _parentTarget.PlayTargetDestroyAudio();

                StartCoroutine(_parentTarget.DestroyTarget());
            }
        }
    }
}
