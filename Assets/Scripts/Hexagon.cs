using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour {

	public float beepSpeed = 2,lightHaloSpeed = 2;
	public float sizeDelta = 0.2f,lightHaloIntensityDelta = 0.2f;
	public GameObject innerHexagon,particleEffect;


//	[HideInInspector]
	public int colorIndex, indexInParent;
	[HideInInspector]
	public bool haloEffect = false;

	SpriteRenderer sp;
	LevelGenerator levelGenerator;
	LevelController levelControllerScript;
	Light haloLight;

	bool beep = false;
	bool makeSmaller = true;
	bool makeDimmer = true;
	float initialIntensity;
	Vector3 initialSize;


	void Start () {
		levelGenerator = this.gameObject.transform.GetComponentInParent<LevelGenerator> ();
		haloLight = GetComponent<Light> ();
		levelControllerScript = levelGenerator.levelControllerScript;
		SetColor (Random.Range (0, levelGenerator.colorList.Count),false);
		initialSize = innerHexagon.transform.localScale;
		initialIntensity = haloLight.intensity;
		haloLight.enabled = false;
	}

	void Update(){
		if (beep) {
			float inc = Time.deltaTime * beepSpeed;
			Vector3 sz =  innerHexagon.transform.localScale;
			if (makeSmaller) {
				sz.x -=	inc;
				sz.y -= inc;
			} else {
				sz.x += inc;
				sz.y += inc;
			}
			innerHexagon.transform.localScale = sz;


			if (innerHexagon.transform.localScale.x < initialSize.x - sizeDelta && makeSmaller) {   
				makeSmaller = false;
			} else {
				if (innerHexagon.transform.localScale.x > initialSize.x && !makeSmaller) {
					makeSmaller = true;
				}
			}
		}

		if (haloEffect) {
			float inc = Time.deltaTime * lightHaloSpeed;
			float intensity = haloLight.intensity;
			if (makeDimmer) {
				intensity -= inc;
			} else {
				intensity += inc;
			}
			haloLight.intensity = intensity;

			if (haloLight.intensity < initialIntensity - lightHaloIntensityDelta && makeDimmer) {
				makeDimmer = false;
			} else {
				if(haloLight.intensity > initialIntensity && !makeDimmer){
					makeDimmer = true;
				}
			}
		}
	}

	public void AdjustLights(bool lightOn,bool haloOn){
		GetComponent<Light> ().enabled = lightOn;
		GetComponent<Hexagon> ().haloEffect = haloOn;
	}

	public void SetColor(int colInd,bool showParticleEffect){
		sp = innerHexagon.GetComponent<SpriteRenderer> ();
		haloLight = GetComponent<Light> ();
		colorIndex = colInd;
		sp.color = levelGenerator.colorList[colorIndex];
		haloLight.color = levelGenerator.colorList [colorIndex];
		if (showParticleEffect) {
			GameObject obj;
			obj = Instantiate (particleEffect, transform.position, Quaternion.identity) as GameObject;
			obj.GetComponent<ParticleEffect> ().SetUpEffect (this.gameObject.transform.GetChild (0).gameObject,levelGenerator.colorList[colorIndex]);
		}
	}

	
	public void StartingHexagonSetup(){
		beep = true;
	}

	public void SetupStartingHexagon(){
		Sprite sp;
		if (levelControllerScript.againstAIMode) {
			sp = levelControllerScript.playerSprite;
		}
		else {
			sp = levelControllerScript.acquiredSprite;
		}
		this.gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().sprite = sp;
		levelGenerator.StopHexagonStartingSetup (indexInParent);
	}

	public void StopHexagonSetup(){
		beep = false;
		Vector3 sz = innerHexagon.transform.localScale;
		sz = initialSize;
		innerHexagon.transform.localScale = sz;
	}

	public void SetObjectActive(){
		this.gameObject.SetActive (true);
	}
		

	public void SetObjectDisabled(){
		this.gameObject.SetActive (false);
	}


	void OnMouseDown(){
		if (levelGenerator.designLevel) {
			string oldStr = levelGenerator.levelDesign;
			string newStr = "";
			for(int i = 0;i<oldStr.Length;i++){
				if(i == indexInParent){
					newStr += '0';
				}
				else{
					newStr += oldStr[i];
				}
			}
			levelGenerator.levelDesign = newStr;


			SetObjectDisabled ();

			levelGenerator.removeHexagonHistory.Push (this.gameObject);
			Debug.Log ("Level Design : " + levelGenerator.levelDesign);

		}

		if (levelControllerScript.startingHexagonSelectingPhase && beep) {
			levelControllerScript.startingHexagonSelectingPhase = false;
			SetupStartingHexagon ();
		}
	}
		

	void OnTriggerEnter(Collider other)
	{
		levelGenerator = this.gameObject.transform.GetComponentInParent<LevelGenerator> ();
		levelControllerScript = levelGenerator.levelControllerScript;

		GameObject obj = other.gameObject;
		int objIndex = obj.GetComponent<Hexagon> ().indexInParent;
		if (!levelControllerScript.adjencyList [indexInParent].Contains (objIndex)) {
			levelControllerScript.adjencyList [indexInParent].Add (objIndex);
	//		Debug.Log (indexInParent + "->" + objIndex);
		}
	}

}
