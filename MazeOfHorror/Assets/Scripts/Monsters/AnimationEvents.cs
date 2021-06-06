using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    MonsterController controller;

    void Start()
    {
        controller = GetComponentInParent<MonsterController>();
    }

    void RotationEnd()
    {
        controller.SetState(MonsterController.State.Scaner);
    }

}
