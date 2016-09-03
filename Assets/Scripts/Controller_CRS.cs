using UnityEngine;
using System.Collections;

public class Controller_CRS : MonoBehaviour 
{
	public int m_framesBetweenSpawns; //number of frames between each spawn
	public float m_velocity; //in units per frame

	public int m_CRSCounter; //number of CRS left
	public int m_framesCounter; //counts the number of frames before the next spawn (replaced by chunks of 0.1 seconds)
	public GameObject m_CRSObject; //CRS prefab to be spawned

	public bool m_isActive; //Whether the spawner is active or not (spawns CRS)


	// Use this for initialization
	void Start () 
	{
		m_isActive = true;
		m_framesCounter = 0;

		//DEBUG//
		Vector3 d=new Vector3(1.0f, 0.0f, 1.0f);
		d.Normalize ();
		Move(gameObject.transform.position, d);
		//END DEBUG//
		InvokeRepeating("UpdateTime", 0.03F, 0.03F);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void UpdateTime()
	{
		if (m_isActive) 
		{
			if ((--m_framesCounter) == 0) 
			{
				if (m_CRSCounter != 0) 
				{
					Object.Instantiate (m_CRSObject, gameObject.transform.position, gameObject.transform.rotation); //spawns a CRS at position facing angle
					m_framesCounter = m_framesBetweenSpawns;
					--m_CRSCounter;
				}
			}
		}
	}

	public void Move(Vector3 position, Vector3 direction)
	{
		m_framesCounter = m_framesBetweenSpawns;
		gameObject.GetComponent<Transform>().position=position;
		gameObject.GetComponent<Transform>().rotation=Quaternion.LookRotation(direction);
		gameObject.GetComponent<Rigidbody>().velocity = direction * m_velocity;
		gameObject.GetComponent<MeshRenderer>().enabled = true;
	}

	public void Stop()
	{
		m_framesCounter = 0;
		gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
		Object.Destroy (gameObject); //Destroy? Hide?
	}
}
