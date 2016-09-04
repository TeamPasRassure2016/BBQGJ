using UnityEngine;
using System.Collections;

public class Terrain_Trigger : MonoBehaviour {

	public GameObject m_CRSController;

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == m_CRSController.tag)
			other.gameObject.SendMessage ("LateAutodestruction");
	}
}
