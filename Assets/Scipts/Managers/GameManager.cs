using UnityEngine;
using UnityEngine.UI;

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
        private WaveManager _waveManager;

        #region Player Info Variables
        // during the game, every player will accumulate points
        private int _playersPoints;
        private int _totalTargetsDestroyed;
        private int _numberOfSessionsPlayed = 0;
        #endregion

        #region Previous State Variables
        public int lastSessionTotalTargets = 0;
        public int lastSessionTotalRounds = 3;
        private int lastSessionBaseline = 1;
        private const int _increaseWaveCount = 1;
        private const int _increaseTargetCount = 1;
        #endregion

        public Canvas _pointCanvas;
        private Text _pointText;
        private Text _gameOverText;
        private Button _nextSessionButton;

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

        public int NumberOfSessionsPlayed
        {
            get { return _numberOfSessionsPlayed; }
            set
            {
                if(value == _numberOfSessionsPlayed + 1)
                    _numberOfSessionsPlayed = value;
            }
        }

        public int TotalNumberOfTargets
        {
            get { return lastSessionTotalTargets; }
            set { lastSessionTotalTargets = value; }
        }

        public int LastSessionTotalRounds
        {
            get { return lastSessionTotalRounds; }
            set { lastSessionTotalRounds = value; }
        }

        public static int IncreaseWaveCount
        {
            get { return _increaseWaveCount; }
        }

        public static int IncreaseTargetCount
        {
            get { return _increaseTargetCount; }
        }

        public int LastSessionBaseline
        {
            get { return lastSessionBaseline; }
            set { lastSessionBaseline = value; }
        }
        #endregion

        #region Unity Mehtods
        // Use this for initialization
        void Awake()
        {
            InitGameManager();
            InitCanvasElements();
            _waveManager = GetComponent<WaveManager>();
        }

        void Start()
        {
            _waveManager.enabled = true;
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
            //_pointText = _pointCanvas.GetComponentInChildren<Text>();
            _pointText = GameObject.Find("PointText").GetComponent<Text>();
            _gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
            _gameOverText.enabled = false;

            // Need to change this, 
            _nextSessionButton = _pointCanvas.GetComponentInChildren<Button>();

            if (_pointCanvas == null || _pointText == null || _nextSessionButton == null)
            {
                Debug.LogError("Point canvas element not found");
            }
        }

        // Intsead of a UI button doing this, I am going to use an Object with trigger Collider; for later
        public void EnableNextSession()
        {
            _waveManager.enabled = true;

            // Instead of Disabling a Button, I will disble the Trigger on object
            // then the end of Session
            _nextSessionButton.gameObject.SetActive(true);
            _nextSessionButton.interactable = false;
            _waveManager.InitNextSessionValues();
        }

        public void EndOfGame(WaveManager waveManager)
        {
            Debug.Log("Game end, Time has ran out");
            waveManager.DestroyAllTargetsOnField();

            _numberOfSessionsPlayed++;

            waveManager.enabled = false;
            _nextSessionButton.gameObject.SetActive(false);
            _nextSessionButton.interactable = false;

            //disable the targets left Text --> enable the Game Over text --> enable the Game Menu
            _gameOverText.enabled = true;
            _pointText.enabled = false;
        }
        #endregion
    }
}
