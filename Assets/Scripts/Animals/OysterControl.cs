using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OysterControl : MonoBehaviour, IAnimalBehaviour
{
    [Title("Animation")]
    public SkeletonAnimation anim;

    private void Start()
    {
        anim.timeScale = Random.Range(0.8f, 1.2f);
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
