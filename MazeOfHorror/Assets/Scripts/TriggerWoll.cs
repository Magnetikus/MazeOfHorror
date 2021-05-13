using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWoll : MonoBehaviour
{
    public bool eventWoll = false;

    private void OnTriggerEnter(Collider other)
    {
        eventWoll = true;
    }

    private void OnTriggerExit(Collider other)
    {
        eventWoll = false;
    }
}
