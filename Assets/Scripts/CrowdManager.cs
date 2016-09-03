using UnityEngine;
using System.Collections;

public enum Shape {
    Circle, Rectangle
}

[System.Serializable]
public struct Rectangle {
    public Vector2 topLeft, bottomRight;
}

[System.Serializable]
public struct Circle {
    public float radius;
}

public class CrowdManager : MonoBehaviour {
    [System.Serializable]
    public struct Params {
        public Shape crowdShape;
        public int crowdSize;
        public Rectangle rectangle;
        public Circle circle;
    }
        
    // Constants
    public const string crowdRootTag = "CrowdRoot";

    // Public fields
    public Params parameters;
    public Protester protesterPrefab;

    // Private fields
    GameObject crowdRoot;
    Protester[] protesters;
    int population = 0;

	void Awake () {
        crowdRoot = GameObject.FindGameObjectWithTag (crowdRootTag);
	}

    public void PopulateCrowd() {
        PopulateCrowdCoroutine();
    }

    public void PopulateCrowdCoroutine() {
        if(population > 0) {
            ClearCrowd ();
        }
        protesters = new Protester[parameters.crowdSize];
        for(int i = 0 ; i != parameters.crowdSize ; ++i) {
            SpawnProtester (i);
        }
        Debug.Log ("Crowd generation complete!");
    }
	
    public void ClearCrowd() {
        Debug.Log ("Started clearing crowd...");
        for(int i = 0 ; i != population ; ++i) {
            GameObject.Destroy (protesters[i].gameObject);
            protesters [i] = null;
        }
        population = 0;
        Debug.Log ("Crowd deletion complete!");
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
            switch(parameters.crowdShape) {
            case Shape.Circle:
                pos = Random.insideUnitCircle * parameters.circle.radius;
                break;
            case Shape.Rectangle:
                pos = new Vector2(Random.Range(parameters.rectangle.topLeft.x, parameters.rectangle.bottomRight.x),
                    Random.Range(parameters.rectangle.bottomRight.y, parameters.rectangle.topLeft.y));
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
        if (parameters.crowdShape == Shape.Circle) {
            Gizmos.color = new Color (0, 1, 0, 0.2f);
            Gizmos.DrawSphere (Vector3.zero, parameters.circle.radius);
        }
    }
}
