using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Fakecast : MonoBehaviour
{
    PlayerVariables var;
    Image dotImage;
    RawImage handImage;

    void Start()
    {
        var = GetComponentInParent<PlayerVariables>();
        dotImage = var.cvar.dot.GetComponent<Image>();
        handImage = var.cvar.hand.GetComponent<RawImage>();
    }

    void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;
        Interactable intComp = obj.GetComponent<Interactable>();

        if (obj.CompareTag("Interactable") && intComp.interactable)
        {
            dotImage.enabled = false;
            handImage.enabled = true;
            Debug.Log("Fakecast hit interactable object");
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.CompareTag("Interactable"))
        {
            handImage.enabled = false;
            dotImage.enabled = true;
            Debug.Log("Fakecast exited interactable object");
        }
    }
}
