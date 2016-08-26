using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

namespace Hydrogen
{
	/// <summary>
	/// Point logger. Basic class for Logging and Getting Player High scores online
	/// </summary>
	public class PointLogger : MonoBehaviour 
	{
		private InputField _userInputField;
		private Button _logPlayerScoreButton;
		private GameObject _failedToLogScoreImage;
		private PlayerScoreList _myPlayerScoreList;


		/* NOTE Delete */
		private string _enteredPlayerName;
		[Tooltip("Delete this. We will be using the score that is Located on the GameManger when Merged")]
		public int score = 2217;

		WWW response;
		const string _ipAddressPattern = "[0-9]+.[0-9]+.[0-9]+.[0-9]+";
		const string _scoreLoggerURL = "http://simplegamesstudio.net/PlayerScoreFinder/PostPlayerScores.php";
		const string _connectionFailedMessage = "Unable to connect to Database. Cannot Log Score.\nMake sure You have Internet connection or Firewall not blocking.";
		private string _ipAddressString = null;

		#region Properties
		public int Score {
			get {
				return score;
			}
		}

		public string EnteredPlayerName {
			get {
				return _enteredPlayerName;
			}
		}

		#endregion

		#region Unity Methods
		void Awake ()
		{
			_userInputField = GetComponentInChildren<InputField> ();
			_logPlayerScoreButton = GetComponentInChildren<Button> ();
			_myPlayerScoreList = FindObjectOfType<PlayerScoreList> ();

			//if your trying to find in a child, then you can just use this.name then the name
			_failedToLogScoreImage = GameObject.Find (this.name +"/FailedImage");
			StartCoroutine (GetPublicIPAddress ());
		}

		void Start()
		{
			_logPlayerScoreButton.onClick.AddListener (()=>{StartCoroutine(SendUserScore());});
			_failedToLogScoreImage.SetActive (false);
			_myPlayerScoreList.enabled = false;
		}
		#endregion

		#region Methods
		// This is used to get the users Public IP address; Used by database to prevent people from putting too many
		// entries on the Database
		private IEnumerator GetPublicIPAddress()
		{
			WWW response = new WWW ("http://checkip.dyndns.org");
			yield return response;

			string htmlResponseText;
			if (response != null) {
				htmlResponseText = response.text;
				Match ipAddressMatch = Regex.Match (htmlResponseText, _ipAddressPattern);

				if (ipAddressMatch.Success)
					_ipAddressString = ipAddressMatch.Value;
				else
					_ipAddressString = _connectionFailedMessage;
			} else
				htmlResponseText = _connectionFailedMessage;
		}

		// Will be used to send users score to the PHP page
		private IEnumerator SendUserScore()
		{
			_logPlayerScoreButton.interactable = false;
			yield return new WaitUntil (()=>{return _ipAddressString != null;});
			_logPlayerScoreButton.interactable = true;

			if(_userInputField.text.Length > 0 && _ipAddressString != null)
			{
				_enteredPlayerName = _userInputField.text;

				WWWForm postReqForm = new WWWForm ();
				postReqForm.AddField ("player_name", _userInputField.text);
				postReqForm.AddField ("public_ip", _ipAddressString);
				postReqForm.AddField ("score", score);

				//id and date is done Auto on PHP page
				response = new WWW(_scoreLoggerURL, postReqForm);
				yield return response;

				if (response == null)
					_failedToLogScoreImage.SetActive (true);
				else {
					_myPlayerScoreList.enabled = true;
				}
			}
		}

		#endregion

	}
}
