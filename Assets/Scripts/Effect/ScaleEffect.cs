using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEffect : MonoBehaviour
{
    public bool playOnAwake = true;

    [SerializeField] TweenSettings<float> scaleSettings;
    Tween currentTween;


    public void Play()
    {
        currentTween = Tween.Scale(transform, scaleSettings);
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
