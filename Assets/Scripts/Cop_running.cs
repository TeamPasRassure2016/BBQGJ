using UnityEngine;
using System.Collections;

public class Cop_running : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MoveTowards(Vector3 velocity)
	{
		GetComponent<Rigidbody>().velocity = velocity;
	}
}
