using UnityEngine;

public class Controller_CRS : MonoBehaviour 
{
	public int m_framesBetweenSpawns; //number of frames between each spawn
	public float m_velocity; //in units per frame

	private GameObject m_gameManager; //gives infos to the game manager
	public GameObject m_CRSObject; //CRS prefab to be spawned

	public bool m_isActive; //Whether the spawner is active or not (spawns CRS)
	public float m_tBetweenUpdates;


	// Use this for initialization
	void Start () 
	{
		m_isActive = true;
		m_gameManager = GameObject.Find("GameManager");

		InvokeRepeating("UpdateTime", m_tBetweenUpdates, m_tBetweenUpdates);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void UpdateTime()
	{
		if (m_isActive) 
		{
			if (m_gameManager.GetComponent<GameManagerScript>().CrsCount > 0) 
				{
					Object.Instantiate (m_CRSObject, gameObject.transform.position, gameObject.transform.rotation); //spawns a CRS at position facing angle
					--m_gameManager.GetComponent<GameManagerScript>().CrsCount;
				}
		}
	}

	public void Move(Vector3 position, Vector3 direction)
	{
		gameObject.GetComponent<Transform>().position=position;
		gameObject.GetComponent<Transform>().rotation=Quaternion.LookRotation(direction);
		gameObject.GetComponent<Rigidbody>().velocity = direction * m_velocity;
		gameObject.GetComponent<MeshRenderer>().enabled = true;
	}

	public void LateAutodestruction()
	{
		Invoke ("Stop", 0.05f);
	}

	public void Stop()
	{
		m_isActive = false;
		Object.Destroy (gameObject); //Destroy? Hide?
	}
}
