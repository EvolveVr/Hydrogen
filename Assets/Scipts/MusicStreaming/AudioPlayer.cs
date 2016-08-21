using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;


namespace Hydrogen
{
	/// <summary>
	/// Audio player. Basic class for playing the streamed music
	/// </summary>
	public class AudioPlayer : MonoBehaviour 
	{

		//private MySqlConnection musicFinderConnection;
		#region Public Private Variables
		private Sprite _playButtonSprite;
		private Sprite _pauseButtonSprite;

		//work on this tomorrow
		private string connectionString;

		//to manipulate audio
		private AudioSource myAudioSource;
		private Image audioButtonImage;

		//for downloading music
		WWW www;
		private Slider musicProgressSlider;
		private Toggle replayToggle;
		private Text musicTimeText;
		private Text totalTimeText;
		private Text artistNameText;
		private Text trackNameText;

		WWW musicData;

		private string jsonMusicDataString;
		private JSONNode jsonMusicData;

		private MusicList myMusicList;
		private Queue<Tuple> musicQueue = new Queue<Tuple> ();
		private Tuple _currentSong;
		private Stack<Tuple> previousSongStack = new Stack<Tuple> ();
		private LinkedList<Tuple> musicQueueLinked = new LinkedList<Tuple> ();


		//private bool isFinishedPlaying = true;
		#endregion


		#region Properties

		public JSONNode GetMusicData 
		{
			get {
				return jsonMusicData;
			}
		}

		#endregion


		#region Unity Methods
		void Awake()
		{
			//1 ) get the audio source
			myAudioSource = GetComponent<AudioSource>();

			//2) get button images
			loadButtonImage();

			//2) laod the sprites
			bool isLoadSuccessful =  loadPlayPauseButtonSprites();
			if (isLoadSuccessful)
			{
				// set the button image to pause
				audioButtonImage.sprite = _pauseButtonSprite;
			}

			musicProgressSlider = GetComponentInChildren<Slider> ();

			//Get text element reference on AudioPlayer canvas
			InitTextElements ();
			myMusicList = FindObjectOfType<MusicList> ();
			replayToggle = GetComponentInChildren<Toggle> ();

			StartCoroutine (CollectMusicData());
		}

		void Start()
		{
			if (jsonMusicData != null) 
			{
				Tuple song = new Tuple (0, jsonMusicData [0] ["TrackPath"]);
				StartCoroutine (UpdateMusic (song));
				_currentSong = song;
			}
		}

		// This updates the Music time slider so user sees how the music is progressing in time
		void Update()
		{
			// No songs in there, why UpdateSlider
			if (myAudioSource.isPlaying) 
			{
				UpdateMusicSlider ();
			}
		}

		#endregion


		#region Methods
		// Use this for initialization
		IEnumerator UpdateMusic (Tuple trackInfo) 
		{
			www = new WWW(trackInfo.TrackPath);
			StartCoroutine (UpdateMusicDownloadProgress(www));
			yield return www;

			myAudioSource.clip = www.audioClip;

			if (www.error != null)
				Debug.Log (www.error);
			else 
			{
				// Update slider value
				musicProgressSlider.maxValue = www.audioClip.length;

				//Update Time text info
				int[] time = TimeConversion (www.audioClip.length);
				totalTimeText.text = String.Format ("{0}:{1}", time[0], time[1]);

				artistNameText.text = "Artist Name:  " + jsonMusicData [trackInfo.Id] ["ArtistName"];
				trackNameText.text = "Track Name:  " + jsonMusicData[trackInfo.Id]["TrackName"];
			}
		}

		//this function is used to update the down load progress on the slider
		IEnumerator UpdateMusicDownloadProgress(WWW www)
		{
			do
			{
				audioButtonImage.fillAmount = www.progress;
				yield return null;

			} while(www.progress < 1f);

			audioButtonImage.fillAmount += 0.1f;
		}

		// this is the public playpause method for playing and pausing music
		// when the Play button is pressed in game
		public void playPauseMusic()
		{
			if (myAudioSource.clip != null) 
			{
				if (myAudioSource.clip.loadState == AudioDataLoadState.Loaded) 
				{
					if (!myAudioSource.isPlaying) {
						myAudioSource.Play ();
					} else if (myAudioSource.isPlaying) {
						myAudioSource.Pause ();
					}
				}
			}
		}

		// this will be another function called by event manager on button
		public void swapAudioPlayPauseImages()
		{
			// Only set the images if they have an image
			if (_playButtonSprite != null && _pauseButtonSprite != null)
			{
				if(!myAudioSource.isPlaying)
				{
					audioButtonImage.sprite = _pauseButtonSprite;
				}
				else
				{
					audioButtonImage.sprite = _playButtonSprite;
				}
			}
		}

		//this function is going to be use to initialize the database
		private bool loadPlayPauseButtonSprites()
		{
			_playButtonSprite = Resources.Load<Sprite>("MusicImages/PlayButtonImage");
			_pauseButtonSprite = Resources.Load<Sprite>("MusicImages/PauseButtonImage");

			if (_playButtonSprite == null || _pauseButtonSprite == null)
			{
				Debug.LogError("The images for swaping play pause button where not found");
				return false;
			}

			return true;
		}

		// load the button image
		private void loadButtonImage()
		{
			//2) get button images
			Button myButton = GetComponentInChildren<Button>();
			if (myButton != null)
			{
				audioButtonImage = myButton.GetComponent<Image>();
			}
		}

		private void InitTextElements()
		{
			Text[] textComponents = GetComponentsInChildren<Text> ();

			foreach(Text textComponent in textComponents)
			{
				switch (textComponent.name)  
				{
				case "ArtistNameText":
					artistNameText = textComponent;
					break;
				case "TrackNameText":
					trackNameText = textComponent;
					break;
				case "TotalMusicTimeText":
					totalTimeText = textComponent;
					break;
				case "CurrentMusicTimeText":
					musicTimeText = textComponent;
					break;
				}
			}
		}

		void UpdateMusicSlider()
		{
			if(myAudioSource.isPlaying)
			{
				musicProgressSlider.value = myAudioSource.time;

				int[] time = TimeConversion (myAudioSource.time);
				musicTimeText.text = String.Format ("{0}:{1}", time[0], time[1]);
			}

			if (myAudioSource.time == myAudioSource.clip.length) {

			}
		}

		public void CheckSongFinished(float value)
		{
			value += 0.05f;
			if (value >= musicProgressSlider.maxValue) 
			{
				if (!replayToggle.isOn) 
				{
					NextInQueue ();
				} else
				{
					audioButtonImage.sprite = _pauseButtonSprite;
					musicProgressSlider.value = 0f;
				}
			}
		}

		// convert time from Seconds to Minutes and Seconds
		int[] TimeConversion(float timeInSeconds)
		{
			int[] timeStandard = new int[2];
			int minutes = (int)(timeInSeconds / 60);
			int seconds = Mathf.RoundToInt (timeInSeconds % 60);

			timeStandard [0] = minutes;
			timeStandard [1] = seconds;
			return timeStandard;
		}

		// This initializes the JSONNode witht eh music data
		IEnumerator CollectMusicData()
		{
			musicData = new WWW ("http://simplegamesstudio.net/MusicFinder/MusicFinder.php");
			yield return musicData;

			// JSON object starts always starts with = [{ and ends with }]
			int startOfJsonData = musicData.text.IndexOf ("[{");
			int endOfJsonData = musicData.text.IndexOf ("}]");
			int length = endOfJsonData - startOfJsonData + 2;

			jsonMusicDataString = musicData.text.Substring (startOfJsonData, length);

			jsonMusicData = JSONNode.Parse (jsonMusicDataString);
			myMusicList.enabled = true;

			Start ();
		}

		private bool MusicDoneDownloading(WWW music)
		{
			return music.isDone;
		}

		public void NextInQueue()
		{
			if (musicQueueLinked.Count > 0) 
			{
				audioButtonImage.sprite = _pauseButtonSprite;
				musicProgressSlider.value = 0f;
				myAudioSource.Pause ();

				//before changing current song, add it to the stack; I dont think I need to worry about this
				// Reason: How many songs are they going to play before turing game off? a few hundred at most
				previousSongStack.Push(_currentSong);

				// now we can add the new song
				//_currentSong = musicQueue.Dequeue ();		// no longer using queue, because no ability to add last
				_currentSong = musicQueueLinked.Last.Value;
				musicQueueLinked.RemoveLast ();

				StartCoroutine (UpdateMusic (_currentSong));
			}
		}

		public void PreviousOnStack()
		{
			if (previousSongStack.Count > 0) 
			{
				audioButtonImage.sprite = _pauseButtonSprite;
				musicProgressSlider.value = 0f;
				myAudioSource.Pause ();

				//before taking previous song and adding it to current slot, 
				// I need to add the current song and add it to the last slot on list.
				musicQueueLinked.AddLast(_currentSong);

				_currentSong = previousSongStack.Pop();
				Debug.Log (_currentSong.Id);

				StartCoroutine (UpdateMusic (_currentSong));
			}
		}

		// the one on the top of the stack is the one playing, take that one off
		public void DeleteFromMusicQueue()
		{
			musicQueue.Dequeue ();
		}

		public void AddToMusicQueue(Tuple trackInfo, Button clickedButton)
		{
			clickedButton.interactable = false;
			musicQueue.Enqueue (trackInfo);
			musicQueueLinked.AddFirst (trackInfo);
		}
		#endregion
	}
}
	