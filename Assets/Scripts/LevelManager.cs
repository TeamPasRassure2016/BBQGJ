using UnityEngine;

public class LevelManager : MonoBehaviour {
    // Constants
    const string levelRootTag = "LevelRoot";

    // Public fields
    public LevelSequence levelSeq;

    // Private fields
    GameObject levelRoot;
    public int currentLevel;

    void Awake() {
        levelRoot = GameObject.FindGameObjectWithTag (levelRootTag);
    }

    // Load the first level
    public void Start() {
        Debug.Log ("Starting level sequence: loading level 0");
        currentLevel = 0;
        levelSeq.levels [0].Generate (levelRoot);
    }

    // Clear the current level and load the next one
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

    // Clear the current level
    public void Clear() {
        Debug.Log ("Clearing level " + currentLevel);
        levelSeq.levels [currentLevel].Clear (levelRoot);
    }
}
