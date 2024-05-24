using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffect : MonoBehaviour
{
    Tween currentTween;

    public CycleMode cycleMode;
    public float duration;

    public Transform pos1;
    public Transform pos2;

    private void OnEnable()
    {
        currentTween = Tween.Position(transform, startValue: pos1.position, endValue: pos2.position, duration: duration, cycles: -1, cycleMode: cycleMode, useUnscaledTime: true);
    }
    private void OnDisable()
    {
        currentTween.Stop();
    }
}
