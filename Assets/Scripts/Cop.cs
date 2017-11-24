using UnityEngine;

public class Cop : MonoBehaviour {

   void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.name == "Protester(Clone)")
        {
            gameObject.GetComponent<Animator>().SetBool("tape", true);
            float pitch = Random.Range(0.7f,1.3f);
            gameObject.GetComponent<AudioSource>().pitch = pitch;
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
