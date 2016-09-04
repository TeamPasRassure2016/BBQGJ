using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    // Constants
    const string levelRootTag = "LevelRoot";

    public LevelSequence levelSeq;

    GameObject levelRoot;
    int currentLevel;

    void Awake() {
        levelRoot = GameObject.FindGameObjectWithTag (levelRootTag);
    }

    public void Start() {
        Debug.Log ("Starting level sequence: loading level 0");
        currentLevel = 0;
        levelSeq.levels [0].Generate (levelRoot);
    }

    public void NextLevel() {
        if(currentLevel < levelSeq.levels.Length - 1) {
            Clear ();
            ++currentLevel;
            Debug.Log ("Loading level " + currentLevel);
            levelSeq.levels [currentLevel].Generate (levelRoot);
        }
        else {
            Debug.Log ("Reached the final level!");
        }
    }

    public void Clear() {
        Debug.Log ("Clearing level " + currentLevel);
        levelSeq.levels [currentLevel].Clear (levelRoot);
    }
}
