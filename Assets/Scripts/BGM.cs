using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public static BGM instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            if (this != instance)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
