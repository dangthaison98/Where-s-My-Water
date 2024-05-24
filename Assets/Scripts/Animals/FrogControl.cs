using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FrogControl : AnimalBehaviour
{
    public Collider2D Collider;
    public MeshRenderer meshRenderer;

    [Title("Animation")]
    public SkeletonAnimation anim;

    private void OnMouseDown()
    {
        if (UIManager.Instance.isUseHammer)
        {
            GameManager.Instance.SpawnHammer(gameObject);
            Collider.enabled = false;
        }
    }
    private void OnMouseUp()
    {
        if (!Collider.enabled) return;

        Vector3Int currentPos = LevelControl.Instance.choiceGrid.WorldToCell(transform.position);

        Vector3Int[] aroundPos = new Vector3Int[6];

        int x = Mathf.Abs(currentPos.x) % 2;
        int y = Mathf.Abs(currentPos.y) % 2;

        if (x == 0 && y == 0)
        {
            aroundPos[0] = new Vector3Int(currentPos.x + 1, currentPos.y - 1, 0);
            aroundPos[1] = new Vector3Int(currentPos.x + 1, currentPos.y + 1, 0);
            aroundPos[2] = new Vector3Int(currentPos.x, currentPos.y + 2, 0);
            aroundPos[3] = new Vector3Int(currentPos.x - 2, currentPos.y + 1, 0);
            aroundPos[4] = new Vector3Int(currentPos.x - 2, currentPos.y - 1, 0);
            aroundPos[5] = new Vector3Int(currentPos.x, currentPos.y - 2, 0);
        }
        else if((x == 1 && y == 1) || (x == 0 && y == 1))
        {
            aroundPos[0] = new Vector3Int(currentPos.x + 2, currentPos.y + 1, 0);
            aroundPos[1] = new Vector3Int(currentPos.x, currentPos.y + 2, 0);
            aroundPos[2] = new Vector3Int(currentPos.x - 1, currentPos.y + 1, 0);
            aroundPos[3] = new Vector3Int(currentPos.x - 1, currentPos.y - 1, 0);
            aroundPos[4] = new Vector3Int(currentPos.x, currentPos.y - 2, 0);
            aroundPos[5] = new Vector3Int(currentPos.x + 2, currentPos.y -1, 0);
        }
        else if(x == 1 && y == 0)
        {
            aroundPos[0] = new Vector3Int(currentPos.x + 1, currentPos.y + 1, 0);
            aroundPos[1] = new Vector3Int(currentPos.x, currentPos.y + 2, 0);
            aroundPos[2] = new Vector3Int(currentPos.x - 2, currentPos.y + 1, 0);
            aroundPos[3] = new Vector3Int(currentPos.x - 2, currentPos.y - 1, 0);
            aroundPos[4] = new Vector3Int(currentPos.x, currentPos.y - 2, 0);
            aroundPos[5] = new Vector3Int(currentPos.x + 1, currentPos.y - 1, 0);
        }

        anim.AnimationName = "frog 3";

        LevelControl.Instance.choiceGrid.ClearAllTiles();
        for (int i = 0; i < aroundPos.Length; i++)
        {
            if (LevelControl.Instance.groundGrid.HasTile(aroundPos[i]) && !Physics2D.CircleCast(LevelControl.Instance.choiceGrid.CellToWorld(aroundPos[i]), 0.4f, Vector2.zero, 0, LayerMask.GetMask("Animal")))
            {
                LevelControl.Instance.choiceGrid.SetTile(aroundPos[i], LevelControl.Instance.choiceTile);
            }
        }

        LevelControl.Instance.animalChoice = this;
    }

    public override void MoveAnimal(Vector2 newPos)
    {
        LevelControl.Instance.choiceGrid.ClearAllTiles();
        anim.AnimationName = "frog 1";
        StartCoroutine(Jump(newPos));
    }
    IEnumerator Jump(Vector3 newPos)
    {
        Vector2 difference = newPos - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);

        anim.AnimationName = "frog 4";
        Collider.isTrigger = true;
        meshRenderer.sortingOrder = 4;


        while (transform.position != newPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, newPos, 10 * Time.deltaTime);
            yield return null;
        }
        anim.AnimationName = "frog 1";
        Collider.isTrigger = false;
        meshRenderer.sortingOrder = 2;
    }
    public override void CancelMoveAnimal()
    {
        LevelControl.Instance.choiceGrid.ClearAllTiles();
        anim.AnimationName = "frog 1";
    }


    public override void GetCollision()
    {
        CancelInvoke();

        anim.AnimationName = "frog 2";

        SoundManager.instance.PlayAlligatorColliSound();

        Invoke(nameof(ReturnIdle), 0.25f);
    }
    void ReturnIdle()
    {
        anim.AnimationName = "frog 1";
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
