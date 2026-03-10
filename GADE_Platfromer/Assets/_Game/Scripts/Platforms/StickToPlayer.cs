using UnityEngine;

public class StickToPlayer : MonoBehaviour
{

    private void OntriggerEnter(Collider other)
    {
        //checks if game object has player tag
        if (other.gameObject.CompareTag("Player"))
        {
            //makes player child of platform so they move together
            other.gameObject.transform.SetParent(transform);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        { 
            //Remove Player object as child
            other.gameObject.transform.SetParent(null); 
        }
    }
}
