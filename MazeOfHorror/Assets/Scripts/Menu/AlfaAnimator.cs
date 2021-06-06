using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlfaAnimator : MonoBehaviour
{
    CanvasGroup group;
    public float targetAlpha;

    void Start()
    {
        group = GetComponent<CanvasGroup>();
    }


    void FixedUpdate()
    {
        if (group.alpha < targetAlpha)
            group.alpha += 0.05f;
        else if (group.alpha > targetAlpha)
            group.alpha -= 0.05f;
    }

}
