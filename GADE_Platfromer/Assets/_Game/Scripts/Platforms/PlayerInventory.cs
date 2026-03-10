using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasSlide = false;
    public bool hasBody = false;

    public GameObject fullPistolVisual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (fullPistolVisual != null) fullPistolVisual.SetActive(false);
    }
    public void CollectPart(CollectibleItem.PartType part)
    {
        if (part == CollectibleItem.PartType.Slide) hasSlide = true;
        if (part == CollectibleItem.PartType.Body) hasBody = true;

        CheckForFullWeapon();
    }

    void CheckForFullWeapon()
    {
        if (hasSlide && hasBody)
        {
            Debug.Log("Pistol Assembled!");
            if (fullPistolVisual != null) fullPistolVisual.SetActive(true);
        }
    }
}
