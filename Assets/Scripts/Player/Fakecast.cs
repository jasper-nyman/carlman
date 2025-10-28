using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Fakecast : MonoBehaviour
{
    // References
    [Header("References")]
    PlayerVariables var;
    Image dotImage;
    RawImage handImage;

    // Do not modify these values directly
    [Header("Do Not Modify")]
    public GameObject focusedObject;

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

        if (obj.CompareTag("Interactable"))
        {
            if (intComp.interactable)
            {
                focusedObject = obj;
                dotImage.enabled = false;
                handImage.enabled = true;
                Debug.Log("Fakecast hit interactable object");
            }
            else
            {
                focusedObject = null;
                handImage.enabled = false;
                dotImage.enabled = true;
                Debug.Log("Fakecast exited interactable object");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.CompareTag("Interactable"))
        {
            focusedObject = null;
            handImage.enabled = false;
            dotImage.enabled = true;
            Debug.Log("Fakecast exited interactable object");
        }
    }

    void Update()
    {
        if (focusedObject != null && Input.GetMouseButtonDown(0))
        {
            Interactable intComp = focusedObject.GetComponent<Interactable>();

            if (intComp.interactable && intComp.grabable && intComp.grabComp.grabable)
            {
                var.heldObject = focusedObject;
                Debug.Log("Grabbbed grabable object");
            }
        }
    }
}
