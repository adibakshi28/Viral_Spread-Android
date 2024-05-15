using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	public AudioClip musicClip;
	public GameObject levelSelectPanel,toggleMusicButton,toggleSoundButton,toggleVibrationButton,levelButton;
	public Sprite musicOnSprite, musicOffSprite, soundOnSprite, soundOffSprite, vibrationOnSprite, vibrationOffSprite;
	public List<Color> levelButtonColors = new List<Color> ();

	int maxLevelUnlocked;
	AudioSource aud;
	GameController gameControllerScript;


	void Awake () {
		gameControllerScript = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}

	void Start (){
		aud = GetComponent<AudioSource> ();

		maxLevelUnlocked = PlayerPrefs.GetInt ("Level");
		for (int i = 0; i < gameControllerScript.levelData.Count; i++) {
			GameObject btn;
			btn = Instantiate (levelButton, transform.position, Quaternion.identity) as GameObject;
			btn.transform.parent = levelSelectPanel.transform;
			btn.GetComponent<Image> ().color = levelButtonColors [i % levelButtonColors.Count];
			LevelSelectButton levelSelectButtonScript;
			levelSelectButtonScript = btn.GetComponent<LevelSelectButton> ();

			Vector3 size = btn.GetComponent<RectTransform> ().localScale;
			size.x = 1;
			size.y = 1;
			btn.GetComponent<RectTransform> ().localScale = size;

			levelSelectButtonScript.level = i + 1;
			if(maxLevelUnlocked >= i + 1){
				levelSelectButtonScript.EnableButton ();
			}
			else{
				levelSelectButtonScript.DisableButton ();
			}
		}

		SetSoundSettings ();
		SetMusicSettings ();
		SetVibrationSettings ();
	}


	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			QuitGame ();
		}
	}

	public void SelectLevel(int level){
		gameControllerScript.PlayBtnAudio ();
		gameControllerScript.curLevel = level;
		SceneManager.LoadScene ("Game");
	}


	public void ToggleSoundSetting(){
		if (PlayerPrefs.GetInt ("Sound") == 1) {
			PlayerPrefs.SetInt ("Sound", 0);
		} 
		else {
			PlayerPrefs.SetInt ("Sound", 1);
		}
		SetSoundSettings ();
		gameControllerScript.PlayBtnAudio ();
	}
	void SetSoundSettings(){
		if (PlayerPrefs.GetInt ("Sound") == 1) {
			toggleSoundButton.transform.GetChild(0).GetComponent<Image> ().sprite = soundOnSprite;
		} 
		else {
			toggleSoundButton.transform.GetChild(0).GetComponent<Image> ().sprite = soundOffSprite;
		}
	}


	public void ToggleMusicSetting(){
		if (PlayerPrefs.GetInt ("Music") == 1) {
			PlayerPrefs.SetInt ("Music", 0);
		} 
		else {
			PlayerPrefs.SetInt ("Music", 1);
		}
		SetMusicSettings ();
		gameControllerScript.PlayBtnAudio ();
	}
	void SetMusicSettings (){
		if (PlayerPrefs.GetInt ("Music") == 1) {
			toggleMusicButton.transform.GetChild(0).GetComponent<Image> ().sprite = musicOnSprite;
			aud.clip = musicClip;
			aud.loop = true;
			aud.Play ();
		}
		else {
			toggleMusicButton.transform.GetChild(0).GetComponent<Image> ().sprite = musicOffSprite;
			aud.Stop ();
		}
	}


	public void ToggleVibrationSetting(){
		if (PlayerPrefs.GetInt ("Vibration") == 1) {
			PlayerPrefs.SetInt ("Vibration", 0);
		} 
		else {
			PlayerPrefs.SetInt ("Vibration", 1);
		}
		SetVibrationSettings ();
		gameControllerScript.PlayBtnAudio ();
	}
	void SetVibrationSettings(){
		if (PlayerPrefs.GetInt ("Vibration") == 1) {
			toggleVibrationButton.transform.GetChild(0).GetComponent<Image> ().sprite = vibrationOnSprite;
		}
		else {
			toggleVibrationButton.transform.GetChild(0).GetComponent<Image> ().sprite = vibrationOffSprite;
		}
	}


	public void AllLevelUnlockButton(){
		PlayerPrefs.SetInt("Level",gameControllerScript.levelData.Count);
		SceneManager.LoadScene("Menu");
	}

	public void QuitGame(){
		gameControllerScript.PlayBtnAudio ();
		Application.Quit ();
	}
}
