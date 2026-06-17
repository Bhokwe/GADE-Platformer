using UnityEngine;

public class CollectibleItem : MonoBehaviour
{

    //will add these back at a later stage
    //public enum PartType { Slide, Body}
    //public PartType partType;

    public int pointValue = 10;


    void Update()
    {
        transform.Rotate(Vector3.up * 60f * Time.deltaTime); // rotate the coin
    }
    void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            if (SFXManager.Instance != null)
            {
                SFXManager.Instance.PlaySFX("COIN");
            }

            GameManager.instance.AddScore(pointValue);

            Debug.Log("We got coins my gee! Total is: " + GameManager.instance.currentScore);

            Destroy(gameObject);
        }
    }



}
