using UnityEngine;
using UnityEngine.UI;

namespace Hydrogen
{
    // Only use is to enable the GameManager
    public class StartGame : MonoBehaviour
    {
        private Canvas _startGameCanvas;
        private GameManager _gameManager;
        private Button _startGameButton;

        void Awake()
        {
            // only ever one GameManager in the scene
            _startGameCanvas = GetComponentInParent<Canvas>();
            _gameManager = FindObjectOfType<GameManager>();

            _startGameButton = GetComponent<Button>();

            //Button needs to enable the Game Manager and disble the canvas
            _startGameButton.onClick.AddListener(
                ()=> 
                {
                    _gameManager.enabled = true;
                    _startGameCanvas.enabled = false;
                }
            );

            // the reason Why I need to do this is to set the button inactive so that 
            // it does not show in the beginning
        }

        void Start()
        {
            DisableNextSessionButton();
        }

        // ill leave this for now; change later
        public void StartGameManager()
        {
            // when we start a Game, we need to:
            // 1) Enable the GameManager
            // 2) disable the Start Game menu
            // 3) play any sounds
            _gameManager.enabled = true;
            _startGameCanvas.enabled = false;

            //aditional things...
        }
        
        public void DisableNextSessionButton()
        {
            Button nextSessionButton = GameObject.Find("NextSessionButton").GetComponent<Button>();
            nextSessionButton.interactable = false;
            nextSessionButton.gameObject.SetActive(false);
        }
    }
}
