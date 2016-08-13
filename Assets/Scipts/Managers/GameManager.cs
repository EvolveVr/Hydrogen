using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Hydrogen
{
    /// <summary>
    /// this is the main objectfor managing the entire game;
    /// this includes, 1) entering the game when player hits button
    /// 2) loading a session, 3) keeping track of points
    /// 4) ending game messages and displaying points, 5) loading start game menu's
    /// 6) Displaying player information such as number of targets left on field
    /// There should only ever be one instance of ths object at any given time; hance singleton
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager;

        // during the game, every player will accumulate points
        private int _playersPoints;
        private int _totalTargetsDestroyed;
        public Canvas _pointCanvas;
        private Text _pointText;


        #region Properties
        public int PlayersPoints
        {
            get { return _playersPoints; }
        }

        public int TotalTargetsDestroyed
        {
            get { return _totalTargetsDestroyed; }
            set { _totalTargetsDestroyed = value; }
        }
        #endregion

        #region Unity Mehtods
        // Use this for initialization
        void Awake()
        {
            InitGameManager();

            GameObject canvas = GameObject.FindGameObjectWithTag("PointCanvas");
            _pointCanvas = canvas.GetComponent<Canvas>();
            _pointText = _pointCanvas.GetComponentInChildren<Text>();
            if (_pointCanvas == null)
                Debug.LogError("Text on Point canvas was not found");
        }
        #endregion

        #region Methods
        void InitGameManager()
        {
            if(gameManager == null)
            {
                gameManager = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                if(gameManager != this)
                {
                    Destroy(this);
                }
            }
        }

        public void AddPoints(int points)
        {
            _playersPoints += points;

            if(_pointText != null)
                _pointText.text = _playersPoints.ToString();
        }

        private void InitCanvasElements()
        {
            GameObject canvas = GameObject.FindGameObjectWithTag("PointCanvas");
            _pointCanvas = canvas.GetComponent<Canvas>();
            _pointText = _pointCanvas.GetComponentInChildren<Text>();
            if (_pointCanvas == null)
                Debug.LogError("Text on Point canvas was not found");
        }
        #endregion
    }
}
