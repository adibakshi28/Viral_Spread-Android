using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour {


	public float lifetime;
	public AudioClip audioClip;
	public GameObject innerEffectObject,lightObject;

	AudioSource aud;
	ParticleSystem.MainModule innerEffect, outerEffect;
	Light lt;


	void Awake(){
		aud = GetComponent<AudioSource> ();
		aud.clip = audioClip;
		if (PlayerPrefs.GetInt ("Sound") == 1) {
			aud.Play ();
		}
	}

	void Start () {
		Destroy (this.gameObject, lifetime);
	}

	public void SetUpEffect(GameObject hexagonParent,Color col){
		this.gameObject.transform.parent = hexagonParent.transform;
		outerEffect = GetComponent<ParticleSystem> ().main;
		innerEffect = innerEffectObject.GetComponent<ParticleSystem> ().main;
		lt = lightObject.GetComponent<Light> ();

		// Set color

		Color outerCol = new Color (col.r*1.1f, col.g*1.1f, col.b*1.1f, 0.5f);
		outerEffect.startColor = outerCol;

		Color innerCol = col;
		innerCol = new Color (col.r, col.g, col.b, 0.15f);
		innerEffect.startColor = innerCol;

		lt.color = col;

		// Set size
		Vector3 sz = this.gameObject.transform.localScale;
		sz.x = 1;
		sz.y = 1;
		sz.z = 1;
		this.gameObject.transform.localScale = sz;
	}


}
