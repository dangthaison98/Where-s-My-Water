using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEffect : MonoBehaviour
{
    public bool playOnAwake = true;
    public bool isReturnStartScale;
    Vector3 startScale;

    [SerializeField] TweenSettings<float> scaleSettings;
    Tween currentTween;

    private void Awake()
    {
        if(isReturnStartScale)
        {
            startScale = transform.localScale;
        }    
    }

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
        if (isReturnStartScale)
        {
            transform.localScale = startScale;
        }
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
