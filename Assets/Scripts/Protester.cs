using UnityEngine;
using System;
//using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Protester : MonoBehaviour {
    public float speed = 1f;
    public float directionChangeProbability = 1f;
    public float moveCycleDuration = 1f;
    public float maxDeviationAngle = 30f;
    public float hotZoneRadius = 3f;

    Rigidbody rigidBody;
    float lastMove;
    System.Random rng;
    Vector2 direction;
    bool inHotZone = true;

    Vector2 toCenter;

    void Awake () {
        rigidBody = GetComponent<Rigidbody>();
        rng = new System.Random ();
        direction = UnityEngine.Random.insideUnitCircle.normalized;
        lastMove = Time.time;
    }

    void FixedUpdate () {
        inHotZone = transform.position.magnitude > hotZoneRadius;
        if(Time.time - lastMove > moveCycleDuration) {
            lastMove = Time.time;
            toCenter = -Vec3ToTopVec2 (transform.position).normalized;
            if(inHotZone) {
                direction = RandomDirectionTowardsCenter ();
            }
            else {
                direction = RandomDirection ();
            }
        }
        rigidBody.AddForce (TopVec2ToVec3 (direction));
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + TopVec2ToVec3(direction));
        Gizmos.color = inHotZone ? Color.green : Color.red;
        Gizmos.DrawLine (transform.position, transform.position + TopVec2ToVec3 (toCenter));

        // Draw hot zone radius sphere
        Gizmos.color = Color.green - new Color(0, 0, 0, 0.8f);
        //Gizmos.DrawSphere (Vector3.zero, hotZoneRadius);
    }

    Vector2 RandomDirectionTowardsCenter() {
        float deviation = (float)(rng.NextDouble ()) * 2 * maxDeviationAngle - maxDeviationAngle;
        Vector3 dir3D = Quaternion.Euler (new Vector3 (0, deviation, 0)) * TopVec2ToVec3(toCenter);
        return Vec3ToTopVec2 (dir3D);
    }

    Vector2 RandomDirection() {
        return UnityEngine.Random.insideUnitCircle.normalized;
    }

    /* Convert a Vector2 in top view coordinates to a Vector3 in world coordinates
     * i.e. Vec2ToTopVec3(x, y) -> Vector2(x, 0, y) */
    static Vector3 TopVec2ToVec3(Vector2 dir) {
        return new Vector3 (dir.x, 0, dir.y);
    }

    /* Convert a Vector3 in world coordinates to a Vector2 in top view coordinates
     * i.e. Vec3ToTopVec2(x, y, z) -> Vector2(x, z) */
    static Vector2 Vec3ToTopVec2(Vector3 v) {
        return new Vector2 (v.x, v.z);
    }
}
