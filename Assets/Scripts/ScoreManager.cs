using UnityEngine;
using System.Collections.Generic;

public class ScoreManager {
    public static float Eval (LevelGenerator level) {
        CrowdManager crowdMgr = GameObject.FindObjectOfType<CrowdManager> ();
        List<Protester> protesters = new List<Protester>();
        foreach(Protester p in crowdMgr.protesters) {
            protesters.Add (p);
        }
            
        List<int> groupSizes = new List<int> ();
        while(protesters.Count > 0) {
            // Pick a random protester
            int randomIdx = Random.Range (0, protesters.Count);
            Protester randomProtester = protesters [randomIdx];
            protesters.RemoveAt (randomIdx);

            /* For every close enough protestor, count it in the group if :
                   * Protestor is closer than a specified range
                   * Raycasting from randomProtestor to it doesn't collide with cops */
            float maxDistance = (level.levelShape == Shape.Circle) ? level.radius : Mathf.Min (level.width, level.height);
            maxDistance /= 5f;

            int count = 1;
            List<int> removeIndices = new List<int> ();
            for(int i = 0 ; i < protesters.Count ; ++i) {
                Protester p = protesters [i];
                if(Vector3.Distance(randomProtester.transform.position, p.transform.position) <= maxDistance) {
                    Ray r = new Ray (randomProtester.transform.position, p.transform.position - randomProtester.transform.position);
                    RaycastHit hit;
                    Physics.Raycast (r, out hit);
                    if(hit.collider.tag == "Protester") {
                        // Protester is in group
                        count += 1;
                        Debug.Log("Found a grouped protester : " + i);
                        removeIndices.Add(i);
                    }
                }
            }
            // Finally, remove grouped protestors from the list
            for(int j = removeIndices.Count-1 ; j >= 0 ; --j) {
                protesters.RemoveAt(removeIndices[j]);
            }
            groupSizes.Add(count);
        }
        float avgGrpSize = (float)(crowdMgr.protesters.Length) / groupSizes.Count;
        return 100f/avgGrpSize;
    }
}
