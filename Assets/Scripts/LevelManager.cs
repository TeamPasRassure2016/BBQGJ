using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    // Constants
    const string levelRootTag = "LevelRoot";

    public LevelGenerator levelGen;

    GameObject levelRoot;

    void Awake() {
        levelRoot = GameObject.FindGameObjectWithTag (levelRootTag);
    }

    public void Generate() {
        levelGen.Generate (levelRoot);
    }

    public void Clear() {
        levelGen.Clear (levelRoot);
    }
}
