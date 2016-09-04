using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Custom/Level Sequence")]
public class LevelSequence : ScriptableObject {
    public LevelGenerator[] levels;

    int currentLevel;
}
