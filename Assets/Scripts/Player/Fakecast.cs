using Unity.VisualScripting;
using UnityEngine;

public class Fakecast : MonoBehaviour
{
    PlayerVariables var;

    void Start()
    {
        var = GetComponentInParent<PlayerVariables>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            var.cvar.dot.enabled = false;
            var.cvar.hand.enabled = true;
            Debug.Log("Fakecast hit interactable object");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            var.cvar.hand.enabled = false;
            var.cvar.dot.enabled = true;
            Debug.Log("Fakecast exited interactable object");
        }
    }

    void Update()
    {
        
    }
}
