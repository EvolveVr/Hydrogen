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
    /// 
    /// There should only ever be one instance of ths object at any given time; hance singleton
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager;

        // during the game, every player will accumulate points
        private int _playersPoints;
        public Canvas _pointCanvas;
        private Text _pointText;


        #region Properties
        public int PlayersPoints
        {
            get { return _playersPoints; }
        }
        #endregion

        #region Unity Mehtods
        // Use this for initialization
        void Awake()
        {
            InitGameManager();

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
            _pointText.text = _playersPoints.ToString();
        }
        #endregion
    }
}
