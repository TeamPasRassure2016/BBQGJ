using UnityEngine;
using System.Collections;

public class SpawnCRS : ScriptableObject {

	public Vector3 direction;
	public Vector3 position;
	public GameObject prefab; //CRS prefab to be spawned
	public GameManagerScript gm;
	
	private bool stop;

	public IEnumerator SpawnLine() {

		stop = false;
		while (gm.CrsCount > 0 && !stop)
		{
			GameObject crs = (GameObject)Instantiate (prefab, position, Quaternion.LookRotation(direction));
			position = position + 1f * (direction);
			--gm.CrsCount;

			// check si la prochaine pos touche un cop
			Collider[] hitColliders = Physics.OverlapSphere(position, 0.3f); // (pos , rayon de la sphere)
			foreach (Collider c in hitColliders)
			{
				//Debug.Log(c.gameObject.name);
				if (c.gameObject.name == "CRS(Clone)" ) {
					Debug.Log("Stop spawning");
					// stop l'exec si c'est un cop
					stop = true;
				}
			}

			yield return new WaitForSeconds(0.1f);
		}
		yield return null;
	}
}
