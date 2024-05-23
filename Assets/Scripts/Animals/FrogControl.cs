using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FrogControl : AnimalBehaviour
{
    [Title("Animation")]
    public SkeletonAnimation anim;

    private void OnMouseDown()
    {
        Vector3Int currentPos = LevelControl.Instance.choiceGrid.WorldToCell(transform.position);

        Vector3Int[] aroundPos = new Vector3Int[6];
        aroundPos[0] = new Vector3Int(currentPos.x -1, currentPos.y +2, 0);
        aroundPos[1] = new Vector3Int(currentPos.x + 1, currentPos.y + 2, 0);
        aroundPos[2] = new Vector3Int(currentPos.x + 2, currentPos.y, 0);
        aroundPos[3] = new Vector3Int(currentPos.x + 1, currentPos.y - 2, 0);
        aroundPos[4] = new Vector3Int(currentPos.x - 1, currentPos.y + 2, 0);
        aroundPos[5] = new Vector3Int(currentPos.x - 2, currentPos.y, 0);

        anim.AnimationName = "frog 3";

        LevelControl.Instance.choiceGrid.ClearAllTiles();
        for (int i = 0; i < aroundPos.Length; i++)
        {
            if (LevelControl.Instance.groundGrid.HasTile(aroundPos[i]))
            {
                LevelControl.Instance.choiceGrid.SetTile(aroundPos[i], LevelControl.Instance.choiceTile);
            }
        }
    }



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
