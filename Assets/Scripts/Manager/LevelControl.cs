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

    public float cameraYPos = -1;
    public float cameraSize = 7.5f;

    [HideInInspector] public int animalCount;

    public AnimalBehaviour animalChoice;

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

        Camera.main.transform.position = new Vector3(0, cameraYPos, -10);
    }

    private void Update()
    {
        if (animalChoice == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (choiceGrid.HasTile(choiceGrid.WorldToCell(clickPos)))
            {
                animalChoice.MoveAnimal(choiceGrid.CellToWorld(choiceGrid.WorldToCell(clickPos)));
            }
            else
            {
                animalChoice.CancelMoveAnimal();
            }
            animalChoice = null;
        }
    }

    public void CheckCompleteLevel()
    {
        animalCount--;

        if(animalCount <= 0)
        {
            if (GameManager.Instance.isHard)
            {
                DataManager.CompleteLevel();
                UIManager.Instance.CompleteLevel();
            }
            else
            {
                GameManager.Instance.ActiveSpawnHardLevel();
            }
        }
    }
}
