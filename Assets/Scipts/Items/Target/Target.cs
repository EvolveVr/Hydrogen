using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hydrogen
{
    /// <summary>
    /// this is a basic class for Target Logic; Make sure that this is attached to object that
    /// has a mesh on it but mesh is a mesh collider and the mesh renderer is turned off. This is used for collision events
    /// </summary>
    public class Target : MonoBehaviour
    {
        protected AudioSource targetAudioSource;

        private Dictionary<GameConstants.TargetPart, GameObject> particleEffects = new Dictionary<GameConstants.TargetPart, GameObject>();
        private const string _pathToParticlePrefabs = "Prefabs/ParticleEffects";

        //How much points to give based on where they hit; play with values then make private const
        [HideInInspector]
        public int multiplierValue = 0;     // Make sure to add logic for multiplier values later

        private MeshRenderer[] targetMeshes;
        private bool isActive = true;

        #region Properties
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        #endregion

        #region Unity Methods
        void Awake()
        {
            targetAudioSource = GetComponent<AudioSource>();
            if (targetAudioSource == null)
                Debug.LogError("The targets audio source was not found");

            GetParticleEffects(particleEffects);
            targetMeshes = GetMeshRenderers();
        }
        #endregion

        #region Methods
        public void PlayTargetDestroyAudio()
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
                    break;
                case GameConstants.TargetPart.Outer:
                    particleEffects.TryGetValue(GameConstants.TargetPart.Outer, out particleEffect);
                    if (particleEffect != null)
                        Instantiate(particleEffect, position, Quaternion.identity);
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

        private MeshRenderer[] GetMeshRenderers()
        {
            MeshRenderer[] myMeshArray = GetComponentsInChildren<MeshRenderer>();
            return myMeshArray;
        }

        public void AddPoints(int pointsToGive)
        {
            if (pointsToGive != 1)
            {
                GameManager.gameManager.AddPoints(pointsToGive);
            }
        }

        public IEnumerator DestroyTarget()
        {
            foreach(var mesh in targetMeshes) {
                mesh.enabled = false;
            }
            yield return new WaitForSeconds(targetAudioSource.clip.length);
            Destroy(this.gameObject);
        }
        #endregion
    }
}
