using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public Text LevelText;
	public Text PeopleText;
	public Text CrsText;
	public Text TimerText;
    public GameObject EndLevelScreen;

	public int PeopleCount;

	//public GameObject CRS_Controller;
	public GameObject CRS_Arrow;

	public GameObject CRSprefab;
	public GameObject GOScreen;
    public LevelManager levelManager;

	private GameObject InstantiatedArrow; //too lazy for script

	public int CrsCount;
	public float timer;

	private bool mouseDown;
	private bool validSpawn = false;
	private Vector2 lastMousePosition;
	private Vector2 instantiatePosition;
    bool scoreScreenDisplayed;

	// Use this for initialization
	void Start () {
        LevelText.text = "Level " + (levelManager.currentLevel + 1).ToString ();
		PeopleText.text = "Manifestants : " + PeopleCount.ToString();

        levelManager.Start ();
        CrsCount = levelManager.levelSeq.levels[levelManager.currentLevel].nCops;
        SetCrsText();
	}
	
	// Update is called once per frame
	void Update () {
		SetCrsText();
		SetTime();

		if (Input.GetMouseButtonDown (0) || mouseDown) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			// Timer Update
			SetTime();

			RaycastHit hit;

			Vector3 pos = Input.mousePosition;
			pos = Camera.main.ScreenToWorldPoint (pos);
			
			// force la valeur en y pour etre a la hauteur des cops 
			pos.y = 1f; 

			// cherche tous les obj autour de la pos
			Collider[] hitColliders = Physics.OverlapSphere(pos, 0.3f); // (pos , rayon de la sphere)
			
			// cherche si on a click sur un cop
			foreach (Collider c in hitColliders)
			{
				//Debug.Log(c.gameObject.name);
				if (c.gameObject.name == "cop(Clone)" ) {
					Debug.Log("Valid spawn spot");
					validSpawn = true; 
				}
			}

			if (Physics.Raycast (ray, out hit)) {
				
				if (!mouseDown) {
					// la fleche
					InstantiatedArrow = Instantiate (CRS_Arrow, new Vector3 (hit.point.x, 0.1f, hit.point.z), Quaternion.LookRotation (new Vector3 (0, 1, 0), new Vector3 (1, 0, 0))) as GameObject;
					mouseDown = true;
					instantiatePosition = new Vector2 (hit.point.x, hit.point.z);
					lastMousePosition = new Vector2 (hit.point.x + 1, hit.point.z);
				} else if (mouseDown) {
					//instantiatedarrow shouldn't be null
					Vector2 nextMousePosition = new Vector2 (hit.point.x, hit.point.z);
					lastMousePosition = nextMousePosition;
					Vector2 direction = lastMousePosition - instantiatePosition;
					direction.Normalize ();
					Vector3 direction3 = new Vector3 (direction.x, 0, direction.y);
					//InstantiatedArrow.gameObject.transform.Rotate (new Vector3 (0, 0, 1), angle);
					InstantiatedArrow.gameObject.transform.rotation=Quaternion.LookRotation (new Vector3 (0, 1, 0), direction3);
				}
			}
		} 
		if (Input.GetMouseButtonUp (0)) 
		{
			// les CRS
			// GameObject instantiatedController=Instantiate (CRS_Controller) as GameObject;
			Vector2 direction = lastMousePosition - instantiatePosition;
			direction.Normalize ();
			// Vector3 direction3 = new Vector3 (direction.x, 0, direction.y);
			//instantiatedController.GetComponent<Controller_CRS>().Move(new Vector3(instantiatePosition.x,0.5f,instantiatePosition.y), direction3);
			// Object.Destroy (InstantiatedArrow);
			mouseDown = false;

			if (validSpawn) {
				SpawnCRS line = (SpawnCRS)ScriptableObject.CreateInstance("SpawnCRS");
				line.direction = new Vector3 (direction.x, 0, direction.y);
				line.position = new Vector3(instantiatePosition.x,0.5f,instantiatePosition.y);
				line.prefab = CRSprefab;
				line.gm = this;
                line.remCops = CrsCount;
				
				StartCoroutine(line.SpawnLine());
				validSpawn = false;
			}
			Object.Destroy (InstantiatedArrow);
		}

		// Game Over check
        if (CrsCount == 0 && !scoreScreenDisplayed) {
            float score = ScoreManager.Eval (levelManager.levelSeq.levels [levelManager.currentLevel]);
            scoreScreenDisplayed = true;
            if(score < levelManager.levelSeq.levels[levelManager.currentLevel].requiredScore) {
                GameOver(score);
            }
            else {
                Text[] EndLevelTexts = EndLevelScreen.GetComponentsInChildren<Text> ();
                foreach(Text t in EndLevelTexts) {
                    if(t.name == "ScoreText") {
                        t.text = "Score: " + score + "\n" +
                            "Required score: " + levelManager.levelSeq.levels[levelManager.currentLevel].requiredScore;
                    }
                }
                EndLevelScreen.SetActive (true);
            }
		}
	}

    public void LoadNextLevel() {
        scoreScreenDisplayed = false;
        EndLevelScreen.SetActive (false);
        levelManager.NextLevel ();
        CrsCount = levelManager.levelSeq.levels [levelManager.currentLevel].nCops;
    }

	void SetCrsText (){
		CrsText.text = "CRS restants : " + CrsCount.ToString();
	}

	void SetTime () {
		timer += Time.deltaTime;

		float minutes = timer / 60; 
		float seconds = timer % 60;

		TimerText.text = string.Format ("{0:00} : {1:00}", minutes, seconds);
	}

    void GameOver(float score) {
		GOScreen.SetActive(true);
        GOScreen.GetComponentInChildren<Text>().text = 
            "You lose.\n" + 
            "Score: " + score + "\n" +
            "Required score: " + levelManager.levelSeq.levels[levelManager.currentLevel].requiredScore;
	}

}
