using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurtleControl : AnimalBehaviour
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
        if (UIManager.Instance.isUseHook)
        {
            GameManager.Instance.SpawnHook(gameObject);
            GetComponent<MeshRenderer>().sortingOrder = 4;
            Collider.enabled = false;
            return;
        }
        else if (UIManager.Instance.isUseHammer)
        {
            UIManager.Instance.ChoiceHammer();
        }

        gameObject.layer = 0;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 10, LayerMask.GetMask("Animal"));
        if (!hit)
        {
            Collider.enabled = false;
            anim.timeScale = 2;
            anim.AnimationName = "rua 2";
            smokeEffect.SetActive(true);

            SoundManager.instance.PlayAlligatorRunSound();

            StartCoroutine(RunOut());
            return;
        }
        else
        {
            if (hit.collider.TryGetComponent<AnimalBehaviour>(out AnimalBehaviour animalBehaviour))
            {
                animalBehaviour.GetCollision();
            }
        }
    }
    private void OnMouseUp()
    {
        if (!Collider.enabled) return;
        gameObject.layer = 3;
    }

    public override void GetCollision()
    {
        CancelInvoke();
        anim.AnimationName = "rua 3";

        SoundManager.instance.PlayAlligatorColliSound();

        Invoke(nameof(ReturnIdle), 0.25f);
    }
    void ReturnIdle()
    {
        anim.AnimationName = "rua 1";
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
    [Range(1, 12)]
    public int rotate = 1;
    private Tilemap tilemap;

    [Button(ButtonHeight = 100)]
    private void BakeAnimal()
    {
        float rotZ1 = rotate * 30;
        transform.rotation = Quaternion.Euler(0, 0, rotZ1);

        if (tilemap == null) { tilemap = FindObjectOfType<Tilemap>(); }
        transform.position = tilemap.CellToWorld(tilemap.WorldToCell(transform.position));
    }
#endif
}
