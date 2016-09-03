using UnityEngine;
using System.Collections;

public class Terrain_Trigger : MonoBehaviour {

	public GameObject m_CRSController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == m_CRSController.tag)
			other.gameObject.SendMessage ("LateAutodestruction");
	}
}
