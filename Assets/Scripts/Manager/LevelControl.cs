using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelControl : MonoBehaviour
{
    public static LevelControl Instance;

    public Tilemap mapGrid;
    public TileBase choiceTile;

    [HideInInspector] public int animalCount;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void CheckCompleteLevel()
    {
        animalCount--;

        if(animalCount <= 0)
        {
            DataManager.CompleteLevel();
            UIManager.Instance.CompleteLevel();
        }
    }
}
