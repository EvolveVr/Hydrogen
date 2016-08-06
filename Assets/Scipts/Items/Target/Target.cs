using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// this is a basic manager for inner middle, and outer targets
    /// </summary>
    /// 

    public class Target : MonoBehaviour
    {
        public AudioClip targetAudioClip;
        public GameObject particleEffect;
        protected AudioSource targetAudioSource;

        //How much points to give based on where they hit; play with values then make private const
        public int centerHitPoints = 10;
        public int middleHitPoints = 5;
        public int outerHitPoints = 2;
        [HideInInspector]
        public int multiplierValue = 0;     // Make sure to add logic for multiplier values later

        // need these for cgiving proper points
        private float _innerSqrMagnitude;
        private float _outerSqrMagnitude;

        #region Unity Methods
        void Awake()
        {
            targetAudioSource = GetComponent<AudioSource>();

            if (targetAudioSource == null)
                Debug.LogError("The targets audio source was not found");

            InitMagnitudes();
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Bullet")
            {
                InitParticleEffect(GameConstants.TargetPart.Inner);

                ContactPoint collision = other.contacts[0];
                Vector3 pos = collision.point;
                Vector3 fromParentToContactVector = (pos - this.transform.position);

                int pointsToGive = CalcPointsByHitVector(fromParentToContactVector, _innerSqrMagnitude, _outerSqrMagnitude);
                
                if(pointsToGive != 1)
                {
                    GameManager.gameManager.AddPoints(pointsToGive);
                }

                Debug.DrawRay(this.transform.position, fromParentToContactVector, Color.cyan, 40.0f);
                Destroy(transform.parent.gameObject);
            }
        }
        #endregion

        #region Methods
        protected void PlayTargetDestroyAudio()
        {
            targetAudioSource.Play();
        }

        // this is for spawnign the particle effect
        public void InitParticleEffect(GameConstants.TargetPart targetPart)
        {
            switch(targetPart)
            {
                case GameConstants.TargetPart.Inner:
                    Instantiate(particleEffect, this.transform.position, Quaternion.identity);
                    Debug.Log("Inner part");
                    break;
                case GameConstants.TargetPart.Middle:
                    Debug.Log("Middle part");
                    break;
                case GameConstants.TargetPart.Outer:
                    Debug.Log("Outer part");
                    break;
            }
        }

        // used to see if inner middle or outer parts of targets were hit; More efficient to not calculate every hit
        void InitMagnitudes()
        {
            foreach(Transform child in transform.parent)
            {
                if(child.name == "InnerTransform")
                {
                    _innerSqrMagnitude = (this.transform.position - child.transform.position).sqrMagnitude;
                }
                else if(child.name == "OuterTransform")
                {
                    _outerSqrMagnitude = (this.transform.position - child.transform.position).sqrMagnitude;
                }
            }
        }

        int CalcPointsByHitVector(Vector3 vectorToContactPoint, float innerSqrMag, float outerSqrMag)
        {
            int pointsToGive = -1;
            float sqrMagnitude = vectorToContactPoint.sqrMagnitude;

            if(sqrMagnitude <= innerSqrMag)
            {
                pointsToGive = centerHitPoints;
            }
            else if(sqrMagnitude > innerSqrMag && sqrMagnitude < outerSqrMag)
            {
                pointsToGive = middleHitPoints;
            }
            else
            {
                pointsToGive = outerHitPoints;
            }

            return pointsToGive;
        }
        #endregion
    }
}
