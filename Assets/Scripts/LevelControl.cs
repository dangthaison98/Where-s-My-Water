using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelControl : MonoBehaviour
{
    public Tilemap mapGrid;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var noZ = new Vector3(pos.x, pos.y);
            Vector3Int mouseCell = mapGrid.WorldToCell(noZ);
            Vector3 worldPos = mapGrid.CellToWorld(mouseCell);
            Debug.LogError(Vector3.Distance(Vector3.zero, worldPos));
        }
    }
}
