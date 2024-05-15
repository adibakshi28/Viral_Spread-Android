using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	public bool againstAIMode = false;
	public int moves = 23,computerStartIndex;
	public float hexUpdateWaitTime = 0.2f;
	public GameObject inputPanel, topPanel, colorInputBtn,canvas,winParticleEffect;
	public Text movesText,playerHexagonCountText,computerHexgonCountText,popUpMoveText;
	public LevelGenerator levelGenerator;
	public Sprite acquiredSprite,playerSprite,computerSprite;
	public List<int> startingIndices = new List<int> ();


	[HideInInspector]
	public List<int>[] adjencyList = new List<int>[1000];
	[HideInInspector]
	public bool startingHexagonSelectingPhase = true,startGame = false;
	[HideInInspector]
	public List<GameObject> hexagonObjects;
	[HideInInspector]
	public List<GameObject> activeHexagons;


	List<GameObject> colorBtnList = new List<GameObject>();
	List<int> acquiredHexagonsIndex = new List<int>();
	List<int> playerHexagonIndex = new List<int>();   // In AI mode
	List<int> computerHexagonIndex = new List<int>();	// In AI mode
	int playerStartIndex;								// In AI mode
	bool playerTurn = true; 							// In AI mode
	int startIndex,playerHexagonCount = 0,computerHexagonCount = 0;
	GameController gameControllerScript;

	void Awake () {
		gameControllerScript = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		LevelData levelData = gameControllerScript.levelData[gameControllerScript.curLevel - 1];

		// Set level values
		againstAIMode = levelData.againstAI;
		moves = levelData.moves;
		levelGenerator.n = levelData.n;
		computerStartIndex = levelData.computerStartIndex;
		levelGenerator.levelDesign = levelData.levelDesign;

		startingIndices.Clear ();
		for (int i = 0; i < levelData.startingIndices.Count; i++) {
			startingIndices.Add (levelData.startingIndices [i]);
		}

		levelGenerator.colorList.Clear ();
		for (int i = 0; i < Mathf.Min(levelData.colors,gameControllerScript.colorList.Count); i++) {
			levelGenerator.colorList.Add (gameControllerScript.colorList[i]);
		}
	}

	void Start () {
		// Correct Size of Input Panel
		RectTransform r;
		r = inputPanel.GetComponent<RectTransform> ();
		Vector2 s = r.sizeDelta;
		s.y = s.y * levelGenerator.controlPanelMargin;
		r.sizeDelta = s;

		// Correct Size of Top Panel
		r = topPanel.GetComponent<RectTransform> ();
		s = r.sizeDelta;
		s.y = s.y * levelGenerator.topPanelMargin;
		r.sizeDelta = s;

		if (againstAIMode) {
			playerHexagonCountText.gameObject.SetActive (true);
			computerHexgonCountText.gameObject.SetActive (true);
			movesText.gameObject.SetActive (false);
			playerHexagonCountText.GetComponent<Text> ().text = "Player : " + playerHexagonCount;
			computerHexgonCountText.GetComponent<Text> ().text = "Computer : " + computerHexagonCount;
			hexagonObjects[computerStartIndex].transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().sprite = computerSprite;

		} 
		else {
			playerHexagonCountText.gameObject.SetActive (false);
			computerHexgonCountText.gameObject.SetActive (false);
			movesText.gameObject.SetActive (true);
			movesText.GetComponent<Text> ().text = "Moves : " + moves;
		}

		if (!levelGenerator.designLevel) {
			canvas.GetComponent<Animator> ().SetBool ("SelectCell", true);
		}

		for (int i = 0; i < levelGenerator.colorList.Count; i++) {
			GameObject obj;
			obj = Instantiate (colorInputBtn, transform.position, Quaternion.identity);
			obj.GetComponent<Image> ().color = levelGenerator.colorList [i];
			obj.GetComponent<ColorButtonScript> ().index = i;
			obj.GetComponent<ColorButtonScript> ().levelControllerObject = this.gameObject;
			obj.transform.parent = inputPanel.transform;

			Vector3 size = obj.GetComponent<RectTransform> ().localScale;
			size.x = 1;
			size.y = 1;
			obj.GetComponent<RectTransform> ().localScale = size;

			colorBtnList.Add (obj);

			obj.GetComponent<Image> ().enabled = false;
		}
		inputPanel.GetComponent<HorizontalLayoutGroup> ().enabled = false;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			MenuButton ();
		}
		if (startGame && againstAIMode) {
			playerHexagonCount = playerHexagonIndex.Count;
			playerHexagonCountText.text = "Player : " + playerHexagonCount;

			computerHexagonCount = computerHexagonIndex.Count;
			computerHexgonCountText.text = "Computer : " + computerHexagonCount;
		}
	}
		

	public void StartGame(int startingIndex){
		canvas.GetComponent<Animator> ().SetBool ("SelectCell", false);

		for (int i = 0; i < activeHexagons.Count; i++) {
			activeHexagons [i].GetComponent<Hexagon> ().AdjustLights (false,false);
		}

		inputPanel.GetComponent<HorizontalLayoutGroup> ().enabled = true;
		for (int i = 0; i < colorBtnList.Count; i++) {
			colorBtnList [i].GetComponent<Image> ().enabled = true;
		}

		if (againstAIMode) {
			playerStartIndex = startingIndex;
			playerHexagonIndex.Add (playerStartIndex);
			computerHexagonIndex.Add (computerStartIndex);
			hexagonObjects [playerStartIndex].GetComponent<Hexagon> ().AdjustLights (true,true);
			hexagonObjects [computerStartIndex].GetComponent<Hexagon> ().AdjustLights (false,false);
		} 
		else {
			startIndex = startingIndex;
			acquiredHexagonsIndex.Add (startIndex);
			hexagonObjects [startIndex].GetComponent<Hexagon> ().AdjustLights (true,false);
		}

		popUpMoveText.text = "Player Move";
		canvas.GetComponent<Animator> ().SetTrigger ("PopUpMoveText");
		playerTurn = true;
		ColorBtnEnabler (true);

		startGame = true;
	}



	bool CheckWin(){
		if (againstAIMode) {
			int totalActiveHexagons = activeHexagons.Count;
			int acqHexagons = playerHexagonIndex.Count + computerHexagonIndex.Count;
			if (acqHexagons != totalActiveHexagons) {
				return false;
			} 
			else {
				if (playerHexagonCount >= computerHexagonCount) {
					return true;
				} 
				else {
					return false;
				}
			}
		} 
		else {
			int curColorIndex = hexagonObjects [startIndex].GetComponent<Hexagon> ().colorIndex;
			for (int i = 0; i < activeHexagons.Count; i++) {
				if (activeHexagons [i].GetComponent<Hexagon> ().colorIndex != curColorIndex) {
					return false;
				}
			}
			return true;
		}
	}

	bool CheckLose(){
		if (againstAIMode) {
			int totalActiveHexagons = activeHexagons.Count;
			int acqHexagons = playerHexagonIndex.Count + computerHexagonIndex.Count;
			if (acqHexagons != totalActiveHexagons) {
				return false;
			} 
			else {
				if (playerHexagonCount >= computerHexagonCount) {
					return false;
				} 
				else {
					return true;
				}
			}
		} 
		else {
			if (moves <= 0) {
				return true;
			}
			return false;
		}
	}
		

	IEnumerator UpdateGame(int inputColor,List<int> hexList){
		Queue<int> q = new Queue<int>();
		bool[] visited = new bool[1000];
		for (int i = 0; i < hexList.Count; i++) {
			q.Enqueue (hexList[i]);
			visited [hexList[i]] = true;
		}

		List<int> newHexagonIndices = new List<int> ();

		while (q.Count != 0) {
			int cur = q.Peek ();
			q.Dequeue ();

			if (newHexagonIndices.Contains (cur)) {
				UpdateHexagon (inputColor, hexagonObjects [cur],playerTurn);
				yield return new WaitForSeconds (hexUpdateWaitTime);
			}
			else {
				UpdateHexagon (inputColor, hexagonObjects [cur],false);
			}
		
			for (int i = 0; i < adjencyList [cur].Count; i++) {
				int v = adjencyList [cur] [i];
				if(!visited[v]){
					visited [v] = true;
					if (againstAIMode) {
						if (playerTurn) {
							if (hexagonObjects [v].GetComponent<Hexagon> ().colorIndex == inputColor && !computerHexagonIndex.Contains(v)) {
								q.Enqueue (v);
								if (!hexList.Contains (v)) {
									hexList.Add (v);
									newHexagonIndices.Add (v);
								}
							}
						} 
						else {
							if (hexagonObjects [v].GetComponent<Hexagon> ().colorIndex == inputColor && !playerHexagonIndex.Contains(v)) {
								q.Enqueue (v);
								if (!hexList.Contains (v)) {
									hexList.Add (v);
									newHexagonIndices.Add (v);
								}
							}
						}
					} 
					else {
						if (hexagonObjects [v].GetComponent<Hexagon> ().colorIndex == inputColor) {
							q.Enqueue (v);
							if (!hexList.Contains (v)) {
								hexList.Add (v);
								newHexagonIndices.Add (v);
							}
						}
					}
				}
			}
		}
			
		StartCoroutine (Check ());
		yield return null;
	}

	void UpdateHexagon(int inputColor,GameObject hexagon,bool showParticleEffect){
		Sprite sp;
		if (againstAIMode) {
			if (playerTurn) {
				sp = playerSprite;
				hexagon.GetComponent<Hexagon> ().AdjustLights (true,true);
			}
			else {
				sp = computerSprite;
				hexagon.GetComponent<Hexagon> ().AdjustLights (false, false);
			}
		}
		else {
			sp = acquiredSprite;
			hexagon.GetComponent<Hexagon> ().AdjustLights (true,false);
		}
		hexagon.GetComponent<Hexagon> ().SetColor (inputColor, showParticleEffect);
		hexagon.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().sprite = sp;
	}
		

	IEnumerator Check(){
		if (CheckWin ()) {
			startGame = false;
			StartCoroutine ("Win");
		} else {
			if (CheckLose ()) {
				startGame = false;
				StartCoroutine ("GameOver");
			}
		}

		if (againstAIMode) {
			if (playerTurn) {
				playerTurn = false;		//````````````````````````````````````````````````````(trouble maker)``````````````````````````````````````````````` 

				yield return new WaitForSeconds (0.5f);

				popUpMoveText.text = "Computer Move";
				canvas.GetComponent<Animator> ().SetTrigger ("PopUpMoveText");

				yield return new WaitForSeconds (1f);
				ComputerPlay ();
			}
			else {
				popUpMoveText.text = "Player Move";
				canvas.GetComponent<Animator> ().SetTrigger ("PopUpMoveText");

				yield return new WaitForSeconds (0.5f);
				playerTurn = true;
				ColorBtnEnabler (true);
			}
		}

		yield return null;
	}



	public void ColorInputButton(int i){
		if (startGame) {

			gameControllerScript.PlayBtnAudio ();

			if (againstAIMode) {
				if (playerTurn) {
					ColorBtnEnabler (false);		// Disable all color btns for multiple calls of UpdateGame coroutines
					StartCoroutine (UpdateGame (i, playerHexagonIndex));
					// Updating score in Update()
				} 
			} 
			else {
				moves--;
				movesText.GetComponent<Text> ().text = "Moves : " + moves;

				StartCoroutine (UpdateGame (i, acquiredHexagonsIndex));
			}
		}
	}


	void ColorBtnEnabler(bool enable){
		for (int i = 0; i < colorBtnList.Count; i++) {
			colorBtnList [i].GetComponent<Button> ().interactable = enable;
		}
	}
		

	void ComputerPlay(){ 
		if(startGame){
			int optimalColor = OptimalMove (true);
			if (optimalColor < 0) {			// Do nothing in case of no Hexagons left to conquer
				optimalColor = Random.Range (0, colorBtnList.Count);
			}
				
			StartCoroutine (UpdateGame (optimalColor, computerHexagonIndex));
			// Updating score in Update()
		}
	}

	int OptimalMove(bool forComputer){			// Returns color index chosen by computer (no optimal color found then return -ev number)
		List<int> optimalMoveCountList = new List<int>();

		for(int inputColor = 0;inputColor<colorBtnList.Count;inputColor++){
			List<int> arr = new List<int> ();
			arr.Clear ();

			if (forComputer) {
				for (int i = 0; i < computerHexagonIndex.Count; i++) {
					arr.Add (computerHexagonIndex [i]);
				}
			} 
			else {
				for (int i = 0; i < playerHexagonIndex.Count; i++) {
					arr.Add (playerHexagonIndex [i]);
				}
			}


			Queue<int> q = new Queue<int>();
			bool[] visited = new bool[1000];
			for (int i = 0; i < arr.Count; i++) {
				q.Enqueue (arr[i]);
				visited [arr[i]] = true;
			}

			while (q.Count != 0) {
				int cur = q.Peek ();
				q.Dequeue ();
				for (int i = 0; i < adjencyList [cur].Count; i++) {
					int v = adjencyList [cur] [i];
					if(!visited[v]){
						visited [v] = true;
						if (forComputer) {
							if (hexagonObjects [v].GetComponent<Hexagon> ().colorIndex == inputColor && !playerHexagonIndex.Contains (v)) {
								q.Enqueue (v);
								if (!arr.Contains (v)) {
									arr.Add (v);
								}
							}
						} 
						else {
							if (hexagonObjects [v].GetComponent<Hexagon> ().colorIndex == inputColor && !computerHexagonIndex.Contains(v)) {
								q.Enqueue (v);
								if (!arr.Contains (v)) {
									arr.Add (v);
								}
							}
						}
					}
				}
			}

			optimalMoveCountList.Add (arr.Count);
		}

		int sameCnt = optimalMoveCountList [0];
		bool allSame = true;
		int mx = 0;
		int ind = -1;
		for (int i = 0; i < optimalMoveCountList.Count; i++) {
	//		Debug.Log ("Color " + i + " count is " + optimalMoveCountList [i]);
			if (mx < optimalMoveCountList [i]) {
				mx = optimalMoveCountList [i];
				ind = i;
			}
			if (optimalMoveCountList [i] != sameCnt) {
				allSame = false;
			}
		}

		if (allSame) {
			return -1;
		}

		return ind;
	}


	public void MenuButton(){
		if (!levelGenerator.designLevel) {
			gameControllerScript.PlayBtnAudio ();
		}
		SceneManager.LoadScene("Menu");
	}
		
	public void ReplayButton(){
		if (!levelGenerator.designLevel) {
			gameControllerScript.PlayBtnAudio ();
		}
		SceneManager.LoadScene ("Game");
	}

	public void NextLevelButton(){
		if (!levelGenerator.designLevel) {
			gameControllerScript.PlayBtnAudio ();
			if (gameControllerScript.curLevel < gameControllerScript.levelData.Count) {
				gameControllerScript.curLevel += 1;
			}
		}
		SceneManager.LoadScene ("Game");
	}

	IEnumerator Win(){
		Debug.Log ("Won");

		for (int i = 0; i < activeHexagons.Count; i++) {
			activeHexagons [i].GetComponent<Hexagon> ().AdjustLights (false,false);
		}

		canvas.GetComponent<Animator> ().SetTrigger ("Win");
	

		if (!levelGenerator.designLevel) {
			PlayerPrefs.SetInt ("Level", Mathf.Max (gameControllerScript.curLevel + 1, PlayerPrefs.GetInt ("Level")));
		}

		yield return new WaitForSeconds(0.75f);

		Instantiate (winParticleEffect, transform.position, Quaternion.identity);
	}

	IEnumerator GameOver(){
		Debug.Log ("Lost");

		for (int i = 0; i < activeHexagons.Count; i++) {
			activeHexagons [i].GetComponent<Hexagon> ().AdjustLights (false,false);
		}

		canvas.GetComponent<Animator> ().SetTrigger ("Lose");
		yield return null;
	}
}
