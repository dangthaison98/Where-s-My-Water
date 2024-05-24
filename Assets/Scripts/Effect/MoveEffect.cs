using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffect : MonoBehaviour
{
    Tween currentTween;

    public Transform pos1;
    public Transform pos2;

    private void OnEnable()
    {
        currentTween = Tween.Position(transform, startValue: pos1.position, endValue: pos2.position, duration: 0.5f, cycles: -1, cycleMode: CycleMode.Yoyo, useUnscaledTime: true);
    }
    private void OnDisable()
    {
        currentTween.Stop();
    }
}
