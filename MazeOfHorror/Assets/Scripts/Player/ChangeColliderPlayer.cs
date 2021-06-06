using UnityEngine;

public class ChangeColliderPlayer : MonoBehaviour
{
    private CharacterController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Movet") || other.CompareTag("NoMovet"))
        {
            controller.radius = 0.2f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Movet") || other.CompareTag("NoMovet"))
        {
            controller.radius = 0.5f;
        }
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

}
