using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishControl : AnimalBehaviour
{
    public GameObject fish1;
    public SkeletonAnimation anim1;
    public Collider2D collider1;
    public GameObject fish2;
    public SkeletonAnimation anim2;
    public Collider2D collider2;

    private void Start()
    {
        LevelControl.Instance.animalCount++;
    }

    private void OnMouseDown()
    {
        fish1.layer = 0;
        fish2.layer = 0;

        RaycastHit2D hit = Physics2D.Raycast(fish1.transform.position, transform.right, Vector2.Distance(fish1.transform.position, fish2.transform.position), LayerMask.GetMask("Animal"));
        if (!hit)
        {
            collider1.enabled = false;
            collider2.enabled = false;
            anim1.AnimationName = "fish 2";
            anim2.AnimationName = "fish 2";

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
        fish1.layer = 3;
        fish2.layer = 3;
    }

    IEnumerator RunOut()
    {
        while (Vector2.Distance(fish1.transform.position, fish2.transform.position) > 1.25f)
        {
            fish1.transform.position += fish1.transform.right * 2 * Time.deltaTime;
            fish2.transform.position += fish2.transform.right * -2 * Time.deltaTime;
            yield return null;
        }
        anim1.AnimationName = "fish 3";
        anim2.AnimationName = "fish 3";
        yield return new WaitForSeconds(1f);
        LevelControl.Instance.CheckCompleteLevel();
        Destroy(gameObject);
    }



#if UNITY_EDITOR
    [Title("Editor"), Space(100)]
    private Tilemap tilemap;

    [Button(ButtonHeight = 100)]
    private void BakeAnimal()
    {
        if (tilemap == null) { tilemap = FindObjectOfType<Tilemap>(); }

        fish1.transform.position = tilemap.CellToWorld(tilemap.WorldToCell(fish1.transform.position));
        collider1.offset = fish1.transform.localPosition;
        fish2.transform.position = tilemap.CellToWorld(tilemap.WorldToCell(fish2.transform.position));
        collider2.offset = fish2.transform.localPosition;


        Vector2 difference1 = fish2.transform.position - fish1.transform.position;
        float rotZ1 = Mathf.Atan2(difference1.y, difference1.x) * Mathf.Rad2Deg;

        fish1.transform.rotation = Quaternion.Euler(0, 0, rotZ1);
        fish2.transform.rotation = Quaternion.Euler(0, 0, rotZ1);

    }
#endif
}
