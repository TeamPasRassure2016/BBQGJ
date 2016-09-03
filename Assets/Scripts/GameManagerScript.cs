using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public Text LevelText;
	public Text PeopleText;
	public Text CrsText;
	public Text TimerText;

	public int currentLevel = 1;
	public int PeopleCount;
	public GameObject CRSprefab;
	public GameObject GOScreen;

	private int CrsCount;
	private float timer;

	// Use this for initialization
	void Start () {
		CrsCount = 5;
		SetCrsText();
		LevelText.text = "Level " + currentLevel.ToString();
		PeopleText.text = "Manifestants : " + PeopleCount.ToString();
	}
	
	// Update is called once per frame
	void Update () {

		// Timer Update
		SetTime();
		
		// Mouse Click check
		if (Input.GetMouseButtonDown(0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			Vector3 pos = Input.mousePosition;
			pos = Camera.main.ScreenToWorldPoint(pos);
			Debug.Log(pos);

			if (Physics.Raycast(ray, out hit) && CrsCount > 0)
			{
				// ici instantier la colonne de CRS avec pour pts de départ (hit.point.x, 0.5f , hit.point.z)
				Instantiate(CRSprefab, new Vector3(hit.point.x, 0.5f , hit.point.z), Quaternion.identity);
				CrsCount = CrsCount - 1;
				SetCrsText();
			}
		}

		// Game Over check
		if (CrsCount == 0) {
			GameOver();
		}

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

	void GameOver() {
		GOScreen.SetActive(true);
	}

}
