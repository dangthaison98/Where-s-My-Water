using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OysterControl : AnimalBehaviour
{
    [Title("Animation")]
    public SkeletonAnimation anim;

    private void Start()
    {
        anim.timeScale = Random.Range(0.8f, 1.2f);
    }

    private void OnMouseDown()
    {
        if(UIManager.Instance.isUseHammer)
        {
            GameManager.Instance.SpawnHammer(gameObject);
        }
    }

    public override void GetCollision()
    {
        CancelInvoke();
        anim.AnimationName = "so2";

        SoundManager.instance.PlayAlligatorColliSound();

        Invoke(nameof(ReturnIdle), 0.25f);
    }
    void ReturnIdle()
    {
        anim.AnimationName = "so1";
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
