using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool interactable;
    public bool touchingFakecast;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fakecast"))
        {
            touchingFakecast = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fakecast"))
        {
            touchingFakecast = false;
        }
    }
}