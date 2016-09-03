using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Rectangle {
    public Vector2 topLeft, bottomRight;
}

[System.Serializable]
public struct Circle {
    public float radius;
}

public class CrowdManager : MonoBehaviour {
    public enum CrowdShape {
        Circle, Rectangle
    }

    // Constants
    const string crowdRootTag = "CrowdRoot";

    // Public fields
    public CrowdShape crowdShape = CrowdShape.Circle;
    public int crowdSize = 15;
    public Rectangle rectangle;
    public Circle circle;
    public Protester protesterPrefab;

    // Private fields
    GameObject crowdRoot;
    System.Random rng;
    Protester[] protesters;
    int population = 0;

	void Awake () {
        crowdRoot = GameObject.FindGameObjectWithTag (crowdRootTag);
        rng = new System.Random ();
	}

    public void PopulateCrowd() {
        StartCoroutine (PopulateCrowdCoroutine());
    }

    public IEnumerator PopulateCrowdCoroutine() {
        if(population > 0) {
            ClearCrowd ();
        }
        protesters = new Protester[crowdSize];
        for(int i = 0 ; i != crowdSize ; ++i) {
            SpawnProtester (i);
            Debug.Log ("Spawned protester" + (i + 1));
        }
        Debug.Log ("Crowd generation complete!");
        yield return null;
    }
	
    public void ClearCrowd() {
        Debug.Log ("Started clearing crowd...");
        for(int i = 0 ; i != population ; ++i) {
            Debug.Log ("Deleting " + protesters[i]);
            GameObject.Destroy (protesters[i].gameObject);
            protesters [i] = null;
        }
        population = 0;
    }

    void SpawnProtester(int idx) {
        float timeLimit = 2f, tStart = Time.time;
        Vector2 pos = new Vector2();
        do {
            // Check that the generation isn't stuck
            if(Time.time > (tStart + timeLimit)) {
                Debug.LogError ("Crowd generation taking too long, aborting");
                return;
            }

            // Generate spawn coordinates
            switch(crowdShape) {
            case CrowdShape.Circle:
                pos = Random.insideUnitCircle * circle.radius;
                break;
            case CrowdShape.Rectangle:
                pos = new Vector2(Random.Range(rectangle.topLeft.x, rectangle.bottomRight.x),
                    Random.Range(rectangle.bottomRight.y, rectangle.topLeft.y));
                break;
            }
        } while (!CheckSpawnCoords (pos));
        Protester newProt = GameObject.Instantiate (protesterPrefab, crowdRoot.transform) as Protester;
        newProt.transform.position = Protester.TopVec2ToVec3 (pos) + new Vector3(0, 0.5f, 0);
        protesters [idx] = newProt;
        population += 1;
    }

    bool CheckSpawnCoords(Vector2 pos) {
        Vector3 pos3D = Protester.TopVec2ToVec3 (pos);
        for(int i = 0 ; i != population ; ++i) {
            if (Vector3.Distance (pos3D, protesters[i].transform.position) < 1f) {
                return false;
            }
        }
        return true;
    }

    void OnDrawGizmos() {
        if (crowdShape == CrowdShape.Circle) {
            Gizmos.color = new Color (0, 1, 0, 0.2f);
            Gizmos.DrawSphere (Vector3.zero, circle.radius);
        }
    }
}
