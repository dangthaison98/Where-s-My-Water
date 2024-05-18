using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MatchGrid : MonoBehaviour
{
#if UNITY_EDITOR
    private Tilemap tilemap;

    [Button(ButtonHeight = 100)]
    private void BakeAnimal()
    {
        if (tilemap == null) { tilemap = FindObjectOfType<Tilemap>(); }
        transform.position = tilemap.CellToWorld(tilemap.WorldToCell(transform.position));
    }
#endif
}
