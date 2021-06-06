using UnityEngine;

public class Exit : MonoBehaviour
{
    private GameControler controller;

    void Start()
    {
        controller = GameObject.Find("Controller").GetComponent<GameControler>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int keys = controller.GetKeys();
            if (keys == 0) controller.PlayerWin();
            else
            {
                controller.Message($"Keys need to be found {keys}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) controller.Message($"");
    }

}
