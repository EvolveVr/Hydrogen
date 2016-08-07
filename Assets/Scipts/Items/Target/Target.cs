using UnityEngine;
using System;
using System.Collections.Generic;

namespace Hydrogen
{
    /// <summary>
    /// this is a basic manager for inner middle, and outer targets
    /// </summary>
    /// 

    public class Target : MonoBehaviour
    {
        public AudioClip targetAudioClip;

        private Dictionary<GameConstants.TargetPart, GameObject> particleEffects = new Dictionary<GameConstants.TargetPart, GameObject>();
        private const string _pathToParticlePrefabs = "Prefabs/ParticleEffects";

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

            InitMagnitudesOnTarget();
            GetParticleEffects(particleEffects);

        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Bullet")
            {
                ContactPoint collision = other.contacts[0];
                Vector3 pos = collision.point;
                Vector3 fromParentToContactVector = (pos - this.transform.position);
                //Need to fix!!!!!!!!!! Not instantiated in the correct place
                Vector3 worldSpaceContactVector = transform.TransformVector(fromParentToContactVector);

                int pointsToGive = CalcPointsByHitVector(fromParentToContactVector, _innerSqrMagnitude, _outerSqrMagnitude);

                if(pointsToGive == centerHitPoints)
                {
                    InitParticleEffect(GameConstants.TargetPart.Inner, worldSpaceContactVector);
                } else if(pointsToGive == middleHitPoints)
                {
                    InitParticleEffect(GameConstants.TargetPart.Middle, worldSpaceContactVector);
                } else
                {
                    InitParticleEffect(GameConstants.TargetPart.Outer, worldSpaceContactVector);
                }

                if (pointsToGive != 1)
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
        public void InitParticleEffect(GameConstants.TargetPart targetPart, Vector3 position)
        {
            GameObject particleEffect;

            switch (targetPart)
            {
                case GameConstants.TargetPart.Inner:
                    particleEffects.TryGetValue(GameConstants.TargetPart.Inner, out particleEffect);
                    if(particleEffect != null)
                        Instantiate(particleEffect, position, Quaternion.identity);
                    break;
                case GameConstants.TargetPart.Middle:
                    particleEffects.TryGetValue(GameConstants.TargetPart.Middle, out particleEffect);
                    if (particleEffect != null)
                        Instantiate(particleEffect, position, Quaternion.identity);
                    Debug.Log("Middle");
                    break;
                case GameConstants.TargetPart.Outer:
                    particleEffects.TryGetValue(GameConstants.TargetPart.Outer, out particleEffect);
                    if (particleEffect != null)
                        Instantiate(particleEffect, position, Quaternion.identity);
                    Debug.Log("Outer");
                    break;
            }
        }

        private void GetParticleEffects(Dictionary<GameConstants.TargetPart, GameObject> particleEffectDict)
        {
            if(particleEffectDict == null) {
                particleEffectDict = new Dictionary<GameConstants.TargetPart, GameObject>();
            }
            GameObject centerParticleEffect = Resources.Load(_pathToParticlePrefabs+ "/Explosion_Center") as GameObject;
            GameObject middleParticleEffect = Resources.Load(_pathToParticlePrefabs + "/Explosion_Middle") as GameObject;
            GameObject outerParticleEffect = Resources.Load(_pathToParticlePrefabs + "/Explosion_Outer") as GameObject;

            particleEffectDict.Add(GameConstants.TargetPart.Inner, centerParticleEffect);
            particleEffectDict.Add(GameConstants.TargetPart.Middle, middleParticleEffect);
            particleEffectDict.Add(GameConstants.TargetPart.Outer, outerParticleEffect);
        }
        // used to see if inner middle or outer parts of targets were hit; More efficient to not calculate every hit
        void InitMagnitudesOnTarget()
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
