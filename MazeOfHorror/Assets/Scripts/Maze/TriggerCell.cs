using UnityEngine;

public class TriggerCell : MonoBehaviour
{
    [HideInInspector]
    public Vector3 positionCell;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cell"))
        {
            positionCell = other.transform.position;
            other.GetComponent<CellMovet>().amountScent = 10f;
        }
    }
}
