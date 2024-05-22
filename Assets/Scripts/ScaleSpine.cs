using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSpine : MonoBehaviour
{
    private void Awake()
    {
        if (Camera.main.aspect >= 0.75f)
            transform.localScale = new Vector3(1.4f * Camera.main.aspect, 1, 1);
    }
}
