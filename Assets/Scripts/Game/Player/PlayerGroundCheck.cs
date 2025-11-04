using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public PlayerVariables var;

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag != "Wall" && other.gameObject.transform.parent.tag != "Enemy")
        {
            var.grounded = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag != "Wall" && other.gameObject.transform.parent.tag != "Enemy")
        {
            var.grounded = false;
        }
    }
}