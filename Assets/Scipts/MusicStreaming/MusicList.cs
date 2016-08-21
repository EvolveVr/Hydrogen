using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections;

namespace Hydrogen
{
	/// <summary>
	/// Music list. 
	/// This controls what songs are displayed the list for 
	/// choosing music to listen to on AudioPlayer.
	/// </summary>
	public class MusicList : MonoBehaviour 
	{
		#region Variables
		private AudioPlayer _myAudioPlayer;
		private RectTransform _musicListRectTransform;
		private GameObject _songPanelPrefab;

		private const float _heightOfSongPanel = 90.0f;
		#endregion


		#region
		void Awake()
		{
			_myAudioPlayer = FindObjectOfType<AudioPlayer> ();
			_musicListRectTransform = GetComponent<RectTransform> ();

			_songPanelPrefab = Resources.Load ("Prefabs/UIPrefabs/SongPanel") as GameObject;
			if (_songPanelPrefab == null)
				Debug.LogError ("The song panel for choosing music was not found");

			//May or May not be bad practice, but Im going to put this in the Awake()
			// this is so I can get the musc
		}
		#endregion

		IEnumerator Start()
		{
			yield return new WaitForSeconds(1.5f);

			InitMusicListContent ();
		}

		void InitMusicListContent()
		{
			JSONNode musicData = _myAudioPlayer.GetMusicData;

			//set the size of it before we start to fill it
			_musicListRectTransform.sizeDelta = new Vector2 (_musicListRectTransform.rect.x, _heightOfSongPanel * (musicData.Count + 2));

			// for each song in the array, create new panel--> Parent under the Content panel --> Get its components --> init them
			for (int i = 0; i < musicData.Count; i++) 
			{
				GameObject songPanel = 
					Instantiate (_songPanelPrefab, this.transform.position, this.transform.rotation, this.transform) as GameObject;

				Text[] textComponents = songPanel.GetComponentsInChildren<Text> ();
				foreach (Text text in textComponents) 
				{
					if (text.name == "ArtistNameText") {
						text.text = musicData [i] ["ArtistName"];
					} else if(text.name == "TrackNameText"){
						text.text = musicData [i] ["TrackName"];
					}
				}

				Button songPanelButton = songPanel.GetComponentInChildren<Button> ();
				Button myButton = songPanelButton;
				myButton.name = "MusicQButton" + i.ToString ();
				// must assign it to a variable before supplying it to listener
				Tuple songTuple = new Tuple (i, musicData[i]["TrackPath"]);
				songPanelButton.onClick.AddListener(() => {_myAudioPlayer.AddToMusicQueue(songTuple, myButton);} );
			}
		}

	
	}
}
