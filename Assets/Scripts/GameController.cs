using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class LevelData{
	public bool againstAI;
	public int moves, n, computerStartIndex,colors;
	public string levelDesign;
	public List<int> startingIndices = new List<int> ();
}
	

public class GameController : MonoBehaviour {

	public int version = 1;
	public AudioClip btnAudio;
	public List<Color> colorList = new List<Color> ();
	public List<LevelData> levelData = new List<LevelData>();

	[HideInInspector]
	public int curLevel = 0;

	AudioSource aud; 	// This Audiosource used for playing button sounds only

	void Start () {
		if (PlayerPrefs.GetInt ("HasPlayed") < 50) {   // Launching the game for first time
			PlayerPrefs.SetInt ("HasPlayed", 100);
			PlayerPrefs.SetInt ("Sound", 1);		// 0-> mute sound ; 1-> play sound
			PlayerPrefs.SetInt("Music",1);			// 0-> mute music ; 1-> play music
			PlayerPrefs.SetInt("Vibration",0);		// 0-> mute vibration ; 1-> play vibration
			PlayerPrefs.SetInt("Level", 1);			// max level unlocked
		}

		if(PlayerPrefs.GetInt("Version") < version){		// Updating to new version 
			
		}

		DontDestroyOnLoad (this.gameObject);
		aud = GetComponent<AudioSource> ();

		SceneManager.LoadScene("Menu");
	}


	public void PlayBtnAudio(){
		aud.clip = btnAudio;
		if (PlayerPrefs.GetInt ("Sound") == 1) {
			aud.Play ();
		}
		if (PlayerPrefs.GetInt ("Vibration") == 1) {
			Handheld.Vibrate ();
		}
	}

}
