using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    public bool playOnAwake = true;

    [SerializeField] TweenSettings<Quaternion> scaleSettings;
    Tween currentTween;


    public void Play()
    {
        currentTween = Tween.LocalRotation(transform, scaleSettings);
    }
    public void Stop()
    {
        currentTween.Stop();
    }


    private void OnEnable()
    {
        if (playOnAwake)
        {
            Play();
        }
    }
    private void OnDisable()
    {
        Stop();
    }
}
