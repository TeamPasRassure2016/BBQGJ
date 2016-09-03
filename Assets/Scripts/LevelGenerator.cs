using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Custom/Level Generator")]
public class LevelGenerator : ScriptableObject {
    // Constants
    const float cameraYClose = 10f;
    const float cameraYMedium = 20f;
    const float cameraYFar = 30f;
    const float copDistance = 2f;

    public enum Size {
        Small, Medium, Large
    }

    // Public fields
    public Size size = Size.Small;
    public float width = 10f, height = 10f, radius = 5f;
    public Protester protesterPrefab;
    public Cop copPrefab;
    public CrowdManager crowdManagerPrefab;
    public CrowdManager.Params crowdParameters;
    public Shape levelShape;

    public void Generate(GameObject root) {
        Debug.Log ("Level generation started");

        // Generate the plane first
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.parent = root.transform;
        plane.transform.localScale = new Vector3 ((float)width / 10f, 1, (float)height / 10f);

        // Add a crowd manager and generate the crowd
        CrowdManager crowdManager = (CrowdManager)GameObject.Instantiate (crowdManagerPrefab, root.transform);
        crowdManager.parameters = crowdParameters;
        crowdManager.protesterPrefab = protesterPrefab;
        crowdManager.PopulateCrowd ();

        // Generate the level limits (cops)
        SpawnCops(root);

        // Place the camera at the appropriate position
        Camera cam = Camera.main;
        cam.transform.position = new Vector3(0, size == Size.Small ? cameraYClose :
            size == Size.Medium ? cameraYMedium : cameraYFar, 0);
        cam.transform.localRotation = Quaternion.Euler (new Vector3 (90f, 0, 0));

        Debug.Log ("Level generation finished");
    }

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
                copTop.transform.position = new Vector3 (-x + i * copDistance, 0f, y);
                copBot.transform.position = new Vector3 (-x + i * copDistance, 0f, -y);
            }
            for(int i = 0 ; i != nCopsY ; ++i) {
                Cop copL = (Cop)GameObject.Instantiate (copPrefab, root.transform),
                    copR = (Cop)GameObject.Instantiate (copPrefab, root.transform);
                copL.transform.position = new Vector3 (-x, 0f, -y + i * copDistance);
                copR.transform.position = new Vector3 ( x, 0f, -y + i * copDistance);
            }
            break;
        }
    }

    /* Clear the level by deleting the crowd and the geometry
     */
    public void Clear(GameObject root) {
        // Delete the crowd
        GameObject crowdManager = GameObject.FindGameObjectWithTag (CrowdManager.crowdRootTag);
        if (crowdManager != null)
            crowdManager.GetComponent<CrowdManager>().ClearCrowd ();

        // Delete all level objects
        foreach(Transform t in root.transform) {
            GameObject.Destroy (t.gameObject);
        }

        Debug.Log ("Cleared level");
    }
}