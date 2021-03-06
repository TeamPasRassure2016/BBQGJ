﻿using UnityEngine;
using System.Collections;

public class SpawnCRS : ScriptableObject {

	public Vector3 direction;
	public Vector3 position;
	public GameObject prefab; //CRS prefab to be spawned
	public GameManagerScript gm;
    public int remCops;
	
	private bool stop;
	private GameObject instanceCRSRunning;
	public IEnumerator SpawnLine() {
		GameObject.Find("Audio Source (grr)").GetComponent<AudioSource>().Play();
		stop = false;
		position = position + 1f * (direction);
		instanceCRSRunning = Instantiate (prefab, position, Quaternion.LookRotation (direction)) as GameObject;
		instanceCRSRunning.GetComponent<BoxCollider>().isTrigger = true;
		instanceCRSRunning.GetComponent<Rigidbody>().velocity = direction*5;
		instanceCRSRunning.name = "runningCop";
		instanceCRSRunning.GetComponent<Rigidbody>().useGravity = false;
		instanceCRSRunning.GetComponent<Animator>().SetTrigger("charge");

        GameObject levelRoot = GameObject.Find("Level");
        while (remCops > 0 && !stop)
		{
            GameObject crs = (GameObject)Instantiate (prefab, position, Quaternion.LookRotation(direction), levelRoot.transform);
			position = position + 1f * (direction);
			--gm.CrsCount;

			// check si la prochaine pos touche un cop
			Collider[] hitColliders = Physics.OverlapSphere(position, 0.4f); // (pos , rayon de la sphere)
			foreach (Collider c in hitColliders)
			{
				//Debug.Log(c.gameObject.name);
				if (c.gameObject.name == "cop(Clone)" ) {
					Debug.Log("Stop spawning");
					// stop l'exec si c'est un cop
					stop = true;
					float audio1Volume = 0.6f;
					if(GameObject.Find("Audio Source (grr)").GetComponent<AudioSource>().volume  > 0.1)
					{
						audio1Volume -= 0.2f * Time.deltaTime;
						GameObject.Find("Audio Source (grr)").GetComponent<AudioSource>().volume = audio1Volume;
					}
					
					Object.Destroy (instanceCRSRunning);
				}
			}

			yield return new WaitForSeconds(0.2f);
		}

		yield return null;
	}
}
