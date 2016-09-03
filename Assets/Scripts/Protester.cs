using UnityEngine;
using System;
//using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Protester : MonoBehaviour {
    public float speed = 1f;
    public float directionChangeProbability = 1f;
    public float moveCycleDuration = 1f;

    Rigidbody rigidBody;
    float lastMove;
	System.Random rng;
	Vector2 direction;

	void Awake () {
	    rigidBody = GetComponent<Rigidbody>();
		rng = new System.Random ();
		direction = UnityEngine.Random.insideUnitCircle.normalized;
		lastMove = Time.time;
	}

	void FixedUpdate () {
		if(Time.time - lastMove > moveCycleDuration) {
			lastMove = Time.time;
			if (rng.NextDouble() < directionChangeProbability) {
				direction = UnityEngine.Random.insideUnitCircle.normalized;
			}
        }
		rigidBody.AddForce (Direction2DTo3D (direction));
	}

	void OnDrawGizmos() {
		Gizmos.DrawLine(transform.position, transform.position + Direction2DTo3D(direction));
	}

	static Vector3 Direction2DTo3D(Vector2 dir) {
		return new Vector3 (dir.x, 0, dir.y);
	}
}
