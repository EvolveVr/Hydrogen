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


        void Awake()
        {
            targetAudioSource = GetComponent<AudioSource>();

            if (targetAudioSource == null)
                Debug.LogError("The targets audio source was not found");
        }

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
    }
}
