using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public float controlPanelMargin = 0.15f,topPanelMargin = 0.075f ,gameBorderMargin = 0.05f;		// 15% height for control panel; 5% border from all sides of game area
	public int n = 10;
	public GameObject hexagon; 
	public string levelDesign;
	public LevelController levelControllerScript;
	public List<Color32> colorList;

	[HideInInspector]
	public bool designLevel ;
	[HideInInspector]
	public List<GameObject> hexagonObjects;

	[HideInInspector]
	public Stack<GameObject> removeHexagonHistory = new Stack<GameObject> ();


	int noOfHexagons = 0;

	void Start () {
		if (levelDesign == "") {
			designLevel = true;
		} 
		else {
			designLevel = false;
		}

		float aspectRatio = Camera.main.aspect;
		float orthographicSize = Camera.main.orthographicSize;
		float screenWidth = aspectRatio * orthographicSize * 2,screenHeight = orthographicSize * 2;        // In game units

		float controlPanelHeight = controlPanelMargin * screenHeight;
		float topPanelHeight = topPanelMargin * screenHeight;
		float borderWidth = gameBorderMargin * screenWidth, borderHeight = gameBorderMargin * screenHeight;

		float gameAreaHeight = screenHeight - controlPanelHeight - topPanelHeight - (2 * borderHeight), gameAreaWidth = screenWidth - (2 * borderWidth);
		Vector2 gameAreaTopLeft = new Vector2 (-screenWidth / 2 + borderWidth, screenHeight / 2 - topPanelHeight - borderHeight);
		Vector2 gameAreaTopRight = new Vector2 (screenWidth / 2 - borderWidth, screenHeight / 2 - topPanelHeight - borderHeight);
		Vector2 gameAreaBottonLeft = new Vector2 (-screenWidth / 2 + borderWidth, -screenHeight / 2 + controlPanelHeight + borderHeight);
		Vector2 gameAreaBottomRight = new Vector2 (screenWidth / 2 - borderWidth, -screenHeight / 2 + controlPanelHeight + borderHeight);

		float a = gameAreaWidth/(Mathf.Sqrt(3)*(n + 0.5f));
		float sizeFactor = Mathf.Sqrt (3) * a;	// Width of hexagon object

		Vector2 firstPos = new Vector2 ((Mathf.Sqrt(3)/2)*a + gameAreaTopLeft.x, gameAreaTopLeft.y - a);

		hexagonObjects = new List<GameObject> ();
		for(int i = 0;i<70;i++){
			
			if (designLevel) {
				if (firstPos.y - (1.5f * a * i) - a < gameAreaBottomRight.y) {            // The last row exceeds bottom game margin
					break;
				}
			}

			for (int j = 0; j < n; j++) {
				
				Vector2 pos = firstPos;
				if (i % 2 == 0) {
					pos.x = pos.x + (Mathf.Sqrt (3) * a * j);
				} 
				else {
					pos.x = pos.x + (Mathf.Sqrt (3) * a * j) + (Mathf.Sqrt (3) / 2 * a);
				}
					
				pos.y = pos.y - (1.5f * a * i);

				GameObject obj;
				obj = Instantiate(hexagon,pos,Quaternion.identity) as GameObject;
				hexagonObjects.Add (obj);

				Vector3 sz = obj.transform.localScale;
				sz = sizeFactor * sz;
				obj.transform.localScale = sz;

				obj.transform.parent = this.gameObject.transform;

				obj.GetComponent<Hexagon> ().indexInParent = noOfHexagons;

				if (designLevel) {
					levelDesign += '1';
				} 
				else {
	/*				if (noOfHexagons >= levelDesign.Length) {    // Just to handle out of bound error in case inputPanel is set too short and smaller level design is provides
						levelDesign += '1';				
					} */
	
					if (levelDesign [noOfHexagons] == '1') {
						obj.GetComponent<Hexagon> ().SetObjectActive ();
						levelControllerScript.activeHexagons.Add (obj);     // Add obj to active hexagons list
					} 
					else {
						obj.GetComponent<Hexagon> ().SetObjectDisabled ();
					}
				}

				levelControllerScript.adjencyList [noOfHexagons] = new List<int> ();    // Initialize the list for current hexagon

				noOfHexagons++;
			}

			if (designLevel) {
	/*			if (firstPos.y - (1.5f * a * i) - a < gameAreaBottomRight.y) {            // The last row exceeds bottom game margin
					break;
				}*/
			}
			else {
				if(hexagonObjects.Count >= levelDesign.Length || firstPos.y - (1.5f * a * i) - a < gameAreaBottomRight.y){
					break;	
				}
			}
		
		}
			
		levelControllerScript.hexagonObjects = hexagonObjects;

		// Set Starting Hexagons
		if(!designLevel){
			for(int i = 0;i<levelControllerScript.startingIndices.Count;i++){
				hexagonObjects [levelControllerScript.startingIndices [i]].GetComponent<Hexagon> ().StartingHexagonSetup ();
			}
		}
			
	}



	public void StopHexagonStartingSetup(int startingIndex){
		for(int i = 0;i<levelControllerScript.startingIndices.Count;i++){
			hexagonObjects [levelControllerScript.startingIndices [i]].GetComponent<Hexagon> ().StopHexagonSetup ();
		}
		levelControllerScript.StartGame (startingIndex);
	}



	void Update(){
		if (designLevel) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (removeHexagonHistory.Count > 0) {
					GameObject obj;
					obj = removeHexagonHistory.Peek ();
					removeHexagonHistory.Pop ();

					string oldStr = levelDesign;
					string newStr = "";
					for(int i = 0;i<oldStr.Length;i++){
						if(i == obj.GetComponent<Hexagon>().indexInParent){
							newStr += '1';
						}
						else{
							newStr += oldStr[i];
						}
					}
					levelDesign = newStr;
					Debug.Log ("Level Design : " + levelDesign);

					obj.GetComponent<Hexagon> ().SetObjectActive ();

				}
				else {
					Debug.Log ("Cant Undo");
				}
			}
		}
	}
}
