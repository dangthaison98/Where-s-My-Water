using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelControl : MonoBehaviour
{
    public static LevelControl Instance;

    public Tilemap groundGrid;
    public Tilemap choiceGrid;
    public TileBase choiceTile;

    public float cameraSize = 7.5f;

    [HideInInspector] public int animalCount;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        if (Camera.main.aspect < 0.5f)
            Camera.main.orthographicSize = cameraSize / Camera.main.aspect  * 0.5f;
        else
            Camera.main.orthographicSize = cameraSize;
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
