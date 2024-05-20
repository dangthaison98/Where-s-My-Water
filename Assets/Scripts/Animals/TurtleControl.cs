using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurtleControl : MonoBehaviour
{
    public Collider2D Collider;

    [Title("Animation")]
    public SkeletonAnimation anim;
    [Title("Effect")]
    public GameObject smokeEffect;

    private void Start()
    {
        LevelControl.Instance.animalCount++;
    }

    private void OnMouseDown()
    {
        gameObject.layer = 0;
        if (!Physics2D.Raycast(transform.position, transform.up, 10, LayerMask.GetMask("Animal")))
        {
            Collider.enabled = false;
            anim.AnimationName = "rua 2";
            smokeEffect.SetActive(true);
            StartCoroutine(RunOut());
            return;
        }
    }
    private void OnMouseUp()
    {
        gameObject.layer = LayerMask.GetMask("Animal");
    }

    IEnumerator RunOut()
    {
        while (Vector2.Distance(transform.position, Vector2.zero) < 10)
        {
            transform.position += transform.up * 5 * Time.deltaTime;
            yield return null;
        }
        LevelControl.Instance.CheckCompleteLevel();
        while (Vector2.Distance(transform.position, Vector2.zero) < 20)
        {
            transform.position += transform.up * 5 * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    [Title("Editor"), Space(100)]
    [Range(1, 6)]
    public int rotate;
    private Tilemap tilemap;

    [Button(ButtonHeight = 100)]
    private void BakeAnimal()
    {
        float rotZ1 = rotate * 60;
        transform.rotation = Quaternion.Euler(0, 0, rotZ1);

        if (tilemap == null) { tilemap = FindObjectOfType<Tilemap>(); }
        transform.position = tilemap.CellToWorld(tilemap.WorldToCell(transform.position));
    }
#endif
}
