using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtonScript : MonoBehaviour {

	[HideInInspector]
	public int index;
	[HideInInspector]
	public GameObject levelControllerObject;

	LevelController levelController;

	void Start(){
		levelController = levelControllerObject.GetComponent<LevelController> ();
	}

	public void OnButtonClick(){
		levelController.ColorInputButton (index);
	}
}
