using UnityEngine;
using System.Collections;

public class Cop : MonoBehaviour {

   void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.name == "Protester(Clone)")
        {
            gameObject.GetComponent<Animator>().SetBool("tape", true);
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
    void OnCollisionExit (Collision col)
    {
        if(col.gameObject.name == "Protester(Clone)")
        {
            gameObject.GetComponent<Animator>().SetBool("tape", false);
        }
    }
}
