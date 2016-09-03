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
	public GameObject prefab;

	private int CrsCount;
	private float timer;

	// Use this for initialization
	void Start () {
		CrsCount = 0;
		SetCrsText();
		LevelText.text = "Level " + currentLevel.ToString();
		PeopleText.text = "Manifestants : " + PeopleCount.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		SetTime();

		 if (Input.GetMouseButtonDown(0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			Vector3 pos = Input.mousePosition;
			pos = Camera.main.ScreenToWorldPoint(pos);
			Debug.Log(pos);

			if (Physics.Raycast(ray, out hit))
			{
				// ici instantier la colonne de CRS avec pour pts de départ (hit.point.x, 0.5f , hit.point.z)
				Instantiate(prefab, new Vector3(hit.point.x, 0.5f , hit.point.z), Quaternion.identity);
			}
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
}
