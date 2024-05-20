using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelControl : MonoBehaviour
{
    public static LevelControl Instance;

    public Tilemap mapGrid;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }
}
