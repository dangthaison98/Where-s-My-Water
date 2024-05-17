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
    }

    public float radius;
    public float angle;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //var noZ = new Vector3(pos.x, pos.y);
            //Vector3Int mouseCell = mapGrid.WorldToCell(noZ);
            //Vector3 worldPos = mapGrid.CellToWorld(mouseCell);
            //Debug.LogError(mouseCell);
            Debug.LogError(Caculate.FindPointInCircle(Vector2.zero, radius, angle));
        }
    }
}
