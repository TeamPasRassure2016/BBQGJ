using UnityEngine;

public class Movement : MonoBehaviour {

public float speed;

    private Rigidbody rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
		Vector3 movement = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        rb.AddForce (movement * speed);
    }
}
