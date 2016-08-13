using UnityEngine;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// This class is for controlling the WaveManager
    /// </summary>
    public class Session : MonoBehaviour
    {
        private float _maxTimeForSession = 900.0f;

        private WaveManager _waveManager;

        void Awake()
        {
            _waveManager = FindObjectOfType<WaveManager>();
        }
    }
}