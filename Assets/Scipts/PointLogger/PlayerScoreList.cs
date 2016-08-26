using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Hydrogen
{
	/// <summary>
	/// Player score list. This is a basic class for containing information
	/// gathered from the PointLogger on Database.
	/// </summary>
	public class PlayerScoreList : MonoBehaviour 
	{
		public struct PlayerInfo: IComparable<PlayerInfo>
		{
			string playerName;
			int playerScore;
			string dateAchieved;

			public PlayerInfo(string name, int score, string date)
			{
				playerName = name;
				playerScore = score;
				dateAchieved = date;
			}

			public string PlayerName {
				get {
					return playerName;
				}
			}

			public int PlayerScore {
				get {
					return playerScore;
				}
			}

			public string DateAchieved {
				get {
					return dateAchieved;
				}
			}

			public int CompareTo(PlayerInfo y)
			{
				return this.PlayerScore.CompareTo (y.PlayerScore);
			}
		}

		private PlayerInfo[] _myPlayerInfo;
		private PlayerInfo _localPlayerInfo;
		private PointLogger _myPointLogger;
		private RectTransform _playerScoreContentRectTransform;

		private WWW _playerScoreResponse;

		// the height of the rect transform
		private GameObject _playerScorePrefabPanel;
		private const float heightOfPlayerScorePanel = 52f;


		#region Unity Methods
		void Awake()
		{
			_myPointLogger = GetComponentInParent<PointLogger> ();
			_playerScoreContentRectTransform = GetComponent<RectTransform> ();
			_playerScorePrefabPanel = Resources.Load<GameObject> ("Prefabs/UIPrefabs/IndividualPlayerScorePanel");
		}

		// Use this for initialization
		IEnumerator Start () 
		{
			StartCoroutine (GatherPlayerInfo());
			yield return new WaitUntil (() => { return _playerScoreResponse.isDone;
			});

			AddScoreInfoPanels (_playerScoreContentRectTransform, _playerScorePrefabPanel, _myPlayerInfo);
		}

		#endregion


		public IEnumerator GatherPlayerInfo()
		{
			WWWForm post = new WWWForm ();
			post.AddField ("score", _myPointLogger.Score);
			_playerScoreResponse = new WWW ("http://simplegamesstudio.net/PlayerScoreFinder/GetPlayerScores.php", post);
			yield return _playerScoreResponse;

			int startOfJsonData = _playerScoreResponse.text.IndexOf ("[{");
			int endOfJsonData = _playerScoreResponse.text.IndexOf ("}]");
			int length = endOfJsonData - startOfJsonData + 2;
			string jsonScoreInfoString = _playerScoreResponse.text.Substring (startOfJsonData, length);

			JSONNode jsonScoreData = JSONNode.Parse (jsonScoreInfoString);

			_myPlayerInfo = new PlayerInfo[jsonScoreData.Count + 1];
			for (int i = 0; i < jsonScoreData.Count; i++) 
			{
				_myPlayerInfo [i] = new PlayerInfo (jsonScoreData[i]["player_name"], jsonScoreData[i]["score"].AsInt, jsonScoreData[i]["date"]);
			}
			_myPlayerInfo [jsonScoreData.Count] = 
				new PlayerInfo (_myPointLogger.EnteredPlayerName, _myPointLogger.Score, DateTime.Now.ToString().Substring(0,10));

			// editions
			Array.Sort (_myPlayerInfo);
			Array.Reverse (_myPlayerInfo);
		}

		//this adds panel prefabs under this gameObject
		private void AddScoreInfoPanels(RectTransform myRectTransform, GameObject playerScorePrefab, PlayerInfo[] playerInfo)
		{
			//1) create a panel for each person, get component for each prefab, add info to it, and then adjust size of the panel
			for (int i =0; i < playerInfo.Length; i++) 
			{
				GameObject playerScorePanel = InitUIPrefab (playerScorePrefab);

				//now get Text Componenets on objects
				Text[] textComponents = playerScorePanel.GetComponentsInChildren<Text>();
				foreach (Text textComponent in textComponents) 
				{
					switch(textComponent.name)
					{
					case "PlayerNameText":
						textComponent.text = playerInfo [i].PlayerName;
						break;
					case "PlayerScoreText":
						textComponent.text = playerInfo [i].PlayerScore.ToString();
						break;
					case "DateAchievedText":
						textComponent.text = "Date:\t\t" + playerInfo [i].DateAchieved;
						break;
					default:
						Debug.LogWarning ("PlayerScoreList found text element with Unknown name");
						break;
					}
				}
			}

			//now modify hieght of content panel
			myRectTransform.sizeDelta = new Vector2(myRectTransform.rect.width, heightOfPlayerScorePanel * playerInfo.Length);
		}



		// Basic utility for instantiating and puttin in correct spot, rotation and under correct parent
		GameObject InitUIPrefab(GameObject prefab)
		{
			GameObject myPrefab = Instantiate(prefab, this.transform) as GameObject;
			myPrefab.transform.rotation = this.transform.rotation;
			myPrefab.transform.position = this.transform.position;
			myPrefab.transform.localScale = Vector3.one;

			return myPrefab;
		}
	}
}
