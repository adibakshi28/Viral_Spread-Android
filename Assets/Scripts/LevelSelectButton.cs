using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour {

	public Text levelNumberText;
	public GameObject blockImage;

	[HideInInspector]
	public int level;

	MenuController menuControllerScript;

	void Start(){
		menuControllerScript = GameObject.FindGameObjectWithTag ("MenuController").GetComponent<MenuController> ();
	}


	public void EnableButton(){
		levelNumberText.text = level.ToString ();
		this.gameObject.GetComponent<Button> ().interactable = true;
		blockImage.SetActive(false);

		menuControllerScript = GameObject.FindGameObjectWithTag ("MenuController").GetComponent<MenuController> ();

		GetComponent<Button> ().onClick.AddListener (delegate {
			menuControllerScript.SelectLevel (level);
		});
	}

	public void DisableButton(){
		levelNumberText.text = level.ToString ();
		this.gameObject.GetComponent<Button> ().interactable = false;
		blockImage.SetActive(true);
	}
	

}
