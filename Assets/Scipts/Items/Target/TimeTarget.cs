using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// This is a basic class for targets that give OR take away Time
    /// </summary>
    public class TimeTarget : MonoBehaviour
    {
        private WaveManager _waveManager;
        //this variable is used to tell whether it is a time target that Give or takes away time
        private bool _giveTime = true;

        public float timeToGiveTake = 10f;
        public float timeToLive = 10f;
        private float timeOfLife = 0f;

        //Get Particle effects and Audio source
        private MeshRenderer _myMeshRenderer;
        private GameObject _particleEffectPrefab;
        private AudioSource _myAudioSource;
        private string giveTimeAudioclipPath = "Sounds/SoundsForGame/Explosions/Future Weapons 2 - Explosions_ 03";

        #region Properties
        // when creating a TimeTarget, I need to be able to set whether it gives or takes time
        public bool GiveTime
        {
            set { _giveTime = value; }
        }
        #endregion

        #region unity Methods
        void Awake()
        {
            _waveManager = FindObjectOfType<WaveManager>();
            _myAudioSource = GetComponentInParent<AudioSource>();
            _myMeshRenderer = GetComponent<MeshRenderer>();
            _myAudioSource.clip = Resources.Load<AudioClip>(giveTimeAudioclipPath);
            _particleEffectPrefab = Resources.Load<GameObject>("Prefabs/ParticleEffects/Explosion_Middle");
        }


        void Update()
        {
            timeOfLife += Time.deltaTime;
            if (timeOfLife >= timeToLive)
            {
                Destroy(transform.parent.gameObject);
                // add particle effect later
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Bullet")
            {
                // Add or take time --> Instantiate particle effect at place of hit --> Audio --> Destroy
                _waveManager.CurrentSessionTime -= timeToGiveTake;
                StartCoroutine(DestroyTimeTarget(_myAudioSource.clip.length + 0.05f, other.transform));
            }
        }
        #endregion

        IEnumerator DestroyTimeTarget(float seconds, Transform hitPoint)
        {
            // Spawn Particle Effect --> Play Audio --> Turn off meshRenderer --> Set Lane Inactive -->wait for audio to play-- > destroy
            Instantiate(_particleEffectPrefab, hitPoint.position, Quaternion.identity);
            _myAudioSource.Play();
            _myMeshRenderer.enabled = false;
            GetComponent<BoxCollider>().enabled = false; // dont want it to be hit consistantly

            yield return new WaitForSeconds(seconds);

            Destroy(transform.parent.gameObject);
        }
    }
}
