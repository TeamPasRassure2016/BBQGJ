using UnityEngine;
using System.Collections;

public class SpawnCRS : ScriptableObject {

	public Vector3 direction;
	public Vector3 position;
	public GameObject prefab; //CRS prefab to be spawned
	public GameManagerScript gm;
    public int remCops;
	
	private bool stop;

	public IEnumerator SpawnLine() {

		stop = false;
		instanceCRSRunning.GetComponent<BoxCollider>().isTrigger = true;
		instanceCRSRunning.GetComponent<Rigidbody>().velocity = direction*5;
		instanceCRSRunning.name = "runningCop";
		instanceCRSRunning.GetComponent<Rigidbody>().useGravity = false;
		instanceCRSRunning.GetComponent<Animator>().SetTrigger("charge");

		while (gm.CrsCount > 0 && !stop)
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
					Object.Destroy (instanceCRSRunning);
				}
			}

			yield return new WaitForSeconds(0.2f);
		}

		yield return null;
	}
}
