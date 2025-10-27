using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public PlayerVariables var;

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Wall") == false)
        {
            var.grounded = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Wall") == false)
        {
            var.grounded = false;
        }
    }
}