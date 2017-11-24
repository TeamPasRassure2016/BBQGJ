using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Level Generator")]
public class LevelGenerator : ScriptableObject {
    // Constants
    const float cameraYClose = 7.45f;
    const float cameraYMedium = 13f;
    const float cameraYFar = 32f;
    const float copDistance = 1f;

    public enum Size {
        Small, Medium, Large
    }

    // Public fields
    public Size size = Size.Small;
    public float width = 10f, height = 10f, radius = 5f;
    public Protester protesterPrefab;
    public Cop copPrefab;
    public CrowdManager crowdManagerPrefab;
    public GameObject groundPrefab;
    [ContextMenuItem("Sane crowd settings", "SanitizeCrowdSettings")]
    public CrowdManager.Params crowdParameters;
    public Shape levelShape;
    public float requiredScore;
    public int nCops;

    public void Generate(GameObject root) {
        Debug.Log ("Level generation started");

        // Generate the ground plane first
		GameObject plane = GameObject.Instantiate(groundPrefab, root.transform) as GameObject;
        plane.transform.localScale = new Vector3 ((float)width / 10f, 1, (float)height / 10f);
        // The ground plane needs a trigger to detect when the cop line goes out of range
        if(levelShape == Shape.Circle) {
            SphereCollider coll = plane.gameObject.AddComponent<SphereCollider> ();
            coll.radius = radius;
            coll.isTrigger = true;
        }
        else {
            BoxCollider coll = plane.gameObject.AddComponent<BoxCollider> ();
            coll.size = new Vector3 (width, 1f, height);
            coll.isTrigger = true;
        }

        GameObject crowdManagerObj = GameObject.FindGameObjectWithTag (CrowdManager.crowdRootTag);
        CrowdManager crowdManager;
        if (crowdManagerObj != null)
            crowdManager = crowdManagerObj.GetComponent<CrowdManager>();
        else 
            // Add a crowd manager and generate the crowd
            crowdManager = (CrowdManager)GameObject.Instantiate (crowdManagerPrefab, root.transform);
        
        crowdManager.parameters = crowdParameters;
        crowdManager.protesterPrefab = protesterPrefab;
        crowdManager.PopulateCrowd ();

        // Generate the level limits (cops)
        SpawnCops(root);

        // Place the camera at the appropriate position
        Camera cam = Camera.main;
        cam.orthographicSize = 
        size == Size.Small ? cameraYClose :
            size == Size.Medium ? cameraYMedium : cameraYFar;
        cam.transform.localRotation = Quaternion.Euler (new Vector3 (90f, 0, 0));

        Debug.Log ("Level generation finished");
    }

    // Spawn cops on the border
    void SpawnCops(GameObject root) {
        switch(levelShape) {
        case Shape.Circle:
            int nCops = (int)(Mathf.PI * radius);
            for(int i = 0 ; i != nCops ; ++i) {
                Cop cop = (Cop)GameObject.Instantiate (copPrefab, root.transform);
                float theta = Mathf.PI * 2f * (float)i / nCops;
                cop.transform.position = new Vector3 (Mathf.Sin(theta), 0f, Mathf.Cos(theta)) * radius;
            }
            break;
        case Shape.Rectangle:
            // First, the horizontal lines
            int nCopsX = (int)((float)width / copDistance) + 1,
                nCopsY = (int)((float)height / copDistance);
            float y = height / 2f,
                  x = width / 2f;
            for(int i = 0 ; i != nCopsX ; ++i) {
                Cop copTop = (Cop)GameObject.Instantiate (copPrefab, root.transform),
                    copBot = (Cop)GameObject.Instantiate (copPrefab, root.transform);
                copTop.transform.position = new Vector3 (-x + i * copDistance, 0.5f, y);
                copTop.transform.rotation = Quaternion.Euler(0, 180, 0);
                copBot.transform.position = new Vector3 (-x + i * copDistance, 0.5f, -y);
            }

            // Then, the vertical ones
            for(int i = 1 ; i != nCopsY ; ++i) {
                Cop copL = (Cop)GameObject.Instantiate (copPrefab, root.transform),
                    copR = (Cop)GameObject.Instantiate (copPrefab, root.transform);
                copL.transform.rotation = Quaternion.Euler(0, 90, 0);
                copL.transform.position = new Vector3 (-x, 0.5f, -y + i * copDistance);
                copR.transform.rotation = Quaternion.Euler(0, -90, 0);
                copR.transform.position = new Vector3 ( x, 0.5f, -y + i * copDistance);
            }
            break;
        }
    }

    // Clear the level by deleting the crowd and the geometry
    public void Clear(GameObject root) {
        // Delete the crowd
        GameObject crowdManager = GameObject.FindGameObjectWithTag (CrowdManager.crowdRootTag);
        if (crowdManager != null)
            crowdManager.GetComponent<CrowdManager>().ClearCrowd ();

        // Delete all level objects
        foreach(Transform t in root.transform) {
            if(t.tag != CrowdManager.crowdRootTag)
                GameObject.Destroy (t.gameObject);
        }
        
        Debug.Log ("Cleared level");
    }
        
    // Set sane default values for crowd spawn settings
    public void SanitizeCrowdSettings() {
        float maxDistance = Mathf.Min (width, height) - 1f;
        crowdParameters.circle.radius = maxDistance / 2f;
        crowdParameters.rectangle.topLeft = new Vector2 (-width/2f + 1f, height/2f - 1f);
        crowdParameters.rectangle.bottomRight = new Vector2 (width/2f - 1f, -height/2f + 1f);
    }

    public float TopY() {
        if(levelShape == Shape.Circle) {
            return radius;
        }
        else {
            return height/2f;
        }
    }

    public float RightX() {
        if(levelShape == Shape.Circle) {
            return radius;
        }
        else {
            return width/2f;
        }
    }
}