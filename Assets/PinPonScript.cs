using UnityEngine;
using System.Collections;

public class PinPonScript : MonoBehaviour {

	private bool wait = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!wait) {
			wait = true;
			StartCoroutine(RandomVolum());
		}	
	}

	IEnumerator RandomVolum() {
		gameObject.GetComponent<AudioSource>().volume = (Random.Range(0f, 0.12f));
		yield return new WaitForSeconds(2f);
		wait = false;
	}
}
