using UnityEngine;
using UnityEditor;
using System.Collections;

[CreateAssetMenu(menuName = "Custom/Level Sequence")]
public class LevelSequence : ScriptableObject {
    [ContextMenuItem("Sane crowd settings (all levels)", "SanitizeAllLevelCrowds")]
    [ContextMenuItem("Set prefab associations", "SetPrefabAssociations")]
    public LevelGenerator[] levels;
    public Protester protesterPrefab;
    public Cop copPrefab;
    public CrowdManager crowdManagerPrefab;
    public GameObject groundPrefab;

    public int currentLevel;

    public void SanitizeAllLevelCrowds() {
        foreach(LevelGenerator level in levels) {
            level.SanitizeCrowdSettings ();
        }
    }

    public void SetPrefabAssociations() {
        foreach(LevelGenerator level in levels) {
            level.protesterPrefab = protesterPrefab;
            level.copPrefab = copPrefab;
            level.crowdManagerPrefab = crowdManagerPrefab;
            level.groundPrefab = groundPrefab;
        }
    }
}
