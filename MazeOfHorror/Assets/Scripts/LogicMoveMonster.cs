using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicMoveMonster : MonoBehaviour
{
    //private Transform transformPos;
    private float speedMoving = 0.3f;
    //private float speedRotation = 2f;

    private TriggerWoll triggerWoll;
    
    //private int mod = 0;

    void Start()
    {
        //transformPos = GetComponent<Transform>();
        triggerWoll = GetComponentInChildren<TriggerWoll>();
    }

    
    void Update()
    {

        if (triggerWoll.eventWoll)
        {
            StopMoved();
            Rotation();
        }
        else
        {
            StopRotation();
            MovingForward();
        }
        
    }

    void MovingForward ()
    {
               
        transform.position += transform.forward * speedMoving * Time.deltaTime;
    }

    void Rotation ()
    {
        int angle = Random.Range(1, 3);
        transform.Rotate (0, 90 * angle, 0);
    }

    void StopMoved ()
    {
        transform.position = transform.position;
    }

    void StopRotation ()
    {
        transform.Rotate(0, 0, 0);
    }
}
