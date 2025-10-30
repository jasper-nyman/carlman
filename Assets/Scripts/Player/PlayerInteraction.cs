using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // References
    [Header("References")]
    public PlayerVariables var;
    GameObject cam;

    // Player interaction settings
    [Header("Interaction Settings")]
    public float interactionRange = 3f;
    public LayerMask interactableLayer;
    public LayerMask obstructionLayer;

    void Start()
    {
        cam = var.cam;
    }
}
