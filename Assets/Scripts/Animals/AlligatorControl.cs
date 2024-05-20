using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AlligatorControl : MonoBehaviour
{
    [Title("Data")]
    [Range(0, 9)]
    public int alligatorLength;

    public Transform checkPoint;
    public CapsuleCollider2D capsuleCollider;
    public GameObject head;
    public GameObject body;

    [Title("Animation")]
    public SkeletonAnimation headAnim;
    public SpriteRenderer bodyRenderer;
    public SkeletonAnimation tailAnim;

    [Title("Sprite")]
    public Sprite normalBody;
    public Sprite yellowBody;
    public Sprite redBody;

    [Title("Effect")]
    public GameObject smokeEffect;

    private Rigidbody2D rb;
    private Vector2[] mainPos = new Vector2[6];
    private Vector3 headPos = new Vector2(-0.5659766f, 0);

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = Vector2.zero;

        LevelControl.Instance.animalCount++;

        if (alligatorLength == 0) return;
        mainPos[0] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), -30);
        mainPos[1] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), 30);
        mainPos[2] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), 90);
        mainPos[3] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), 150);
        mainPos[4] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), 210);
        mainPos[5] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), 270);
    }

    private void OnMouseDown()
    {
        gameObject.layer = 0;

        headAnim.AnimationName = "run3";
        bodyRenderer.sprite = yellowBody;
        tailAnim.AnimationName = "run3";
    }
    private void OnMouseDrag()
    {
        CaculatePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), true);
    }
    private void OnMouseUp()
    {
        CaculatePosition(LevelControl.Instance.mapGrid.CellToWorld(LevelControl.Instance.mapGrid.WorldToCell(checkPoint.position)), false);

        if (!Physics2D.Raycast(checkPoint.position, transform.right, 10, LayerMask.GetMask("Animal")))
        {
            capsuleCollider.enabled = false;

            headAnim.AnimationName = "run5";
            headAnim.timeScale = 2;
            bodyRenderer.sprite = normalBody;
            tailAnim.AnimationName = "run5";
            tailAnim.timeScale = 2;
            smokeEffect.SetActive(true);

            StartCoroutine(RunOut());
            return;
        }
        ////////////////////////////////////////////////
        ReturnIdle();
        bodyRenderer.sprite = normalBody;

        gameObject.layer = 3;
    }

    IEnumerator RunOut()
    {
        while (Vector2.Distance(transform.position, Vector2.zero) < 10)
        {
            transform.position += transform.right * 5 * Time.deltaTime;
            yield return null;
        }
        LevelControl.Instance.CheckCompleteLevel();
        while (Vector2.Distance(transform.position, Vector2.zero) < 20)
        {
            transform.position += transform.right * 5 * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.layer == 3)
        {
            headAnim.AnimationName = "run4";
            bodyRenderer.sprite = yellowBody;
            tailAnim.AnimationName = "run4";
        }
    }
    void ReturnIdle()
    {
        int randomNum = UnityEngine.Random.Range(0, 2);
        if (randomNum == 0)
        {
            headAnim.AnimationName = "run1";
            tailAnim.AnimationName = "run1";
        }
        else
        {
            headAnim.AnimationName = "run2";
            tailAnim.AnimationName = "run2";
        }
        bodyRenderer.sprite = normalBody;
    }


    private void CaculatePosition(Vector3 pos, bool isUpdate)
    {
        Vector2 difference = pos - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if (isUpdate)
        {
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotZ, 50 * Time.deltaTime));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
        
        if (alligatorLength == 0) return;

        //Resize Animal
        if ((rb.rotation >= 0 && rb.rotation < 30) || (rb.rotation >= 330 && rb.rotation < 360))
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[0], mainPos[1]);
        }
        else if (rb.rotation >= 30 && rb.rotation < 90)
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[1], mainPos[2]);
        }
        else if (rb.rotation >= 90 && rb.rotation < 150)
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[2], mainPos[3]);
        }
        else if ((rb.rotation >= 150 && rb.rotation < 210))
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[3], mainPos[4]);
        }
        else if (rb.rotation >= 210 && rb.rotation < 270)
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[4], mainPos[5]);
        }
        else
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[5], mainPos[0]);
        }

        head.transform.localPosition = checkPoint.localPosition + headPos;
        body.transform.localScale = new Vector3((head.transform.localPosition.x - 0.3f) * 4, 1, 1);

        capsuleCollider.offset = new Vector2((head.transform.localPosition.x + 0.5659766f) / 2, 0);
        capsuleCollider.size = new Vector2(head.transform.localPosition.x + 1.0659766f, 0.5f);
    }


#if UNITY_EDITOR
    [Title("Editor"), Space(100)]
    public GameObject tail;
    public int rotate;
    public bool isFlip;
    private Tilemap tilemap;

    [Button(ButtonHeight = 100)]
    private void BakeAnimal()
    {
        if(tilemap == null) { tilemap = FindObjectOfType<Tilemap>(); }
        if(isFlip)
        {
            head.transform.localRotation = Quaternion.Euler(180, 0f, 0f);
            body.transform.localRotation = Quaternion.Euler(180, 0f, 0f);
            tail.transform.localRotation = Quaternion.Euler(180, 0f, 0f);
        }
        else
        {
            head.transform.localRotation = Quaternion.identity;
            body.transform.localRotation = Quaternion.identity;
            tail.transform.localRotation = Quaternion.identity;
        }

        transform.position = tilemap.CellToWorld(tilemap.WorldToCell(transform.position));

        if (alligatorLength != 0)
        {
            mainPos[0] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), -30);
            mainPos[1] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), 30);
            mainPos[2] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), 90);
            mainPos[3] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), 150);
            mainPos[4] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), 210);
            mainPos[5] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (alligatorLength + 1), 270);
            float rotZ1 = 90f + Mathf.Clamp(rotate, 0, (alligatorLength * 6f + 6f) - 1) * 360f / (alligatorLength * 6f + 6f);
            transform.rotation = Quaternion.Euler(0, 0, rotZ1);

            if (rotZ1 >= 330 && rotZ1 < 390)
            {
                checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                    mainPos[0], mainPos[1]);
            }
            else if (rotZ1 >= 390 && rotZ1 < 450)
            {
                checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                    mainPos[1], mainPos[2]);
            }
            else if (rotZ1 >= 90 && rotZ1 < 150)
            {
                checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                    mainPos[2], mainPos[3]);
            }
            else if ((rotZ1 >= 150 && rotZ1 < 210))
            {
                checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                    mainPos[3], mainPos[4]);
            }
            else if (rotZ1 >= 210 && rotZ1 < 270)
            {
                checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                    mainPos[4], mainPos[5]);
            }
            else
            {
                checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                    mainPos[5], mainPos[0]);
            }

            Vector2 difference = tilemap.CellToWorld(tilemap.WorldToCell(checkPoint.position))- tilemap.CellToWorld(tilemap.WorldToCell(transform.position));
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

            head.transform.localPosition = checkPoint.localPosition + headPos;
            body.SetActive(true);
            body.transform.localScale = new Vector3((head.transform.localPosition.x - 0.3f) * 4, 1, 1);

            capsuleCollider.offset = new Vector2((head.transform.localPosition.x + 0.5659766f) / 2, 0);
            capsuleCollider.size = new Vector2(head.transform.localPosition.x + 1.0659766f, 0.5f);
        }
        else
        {
            head.transform.localPosition = new Vector3(0.3f + 0.8659766f * alligatorLength, 0);
            checkPoint.transform.localPosition = new Vector3(0.8659766f * (alligatorLength + 1), 0);
            body.SetActive(false);

            capsuleCollider.offset = new Vector2(0.8659766f * (alligatorLength + 1) / 2, 0);
            capsuleCollider.size = new Vector2(0.8659766f * (alligatorLength + 1) + 0.5f, 0.5f);


            transform.rotation = Quaternion.Euler(0, 0, 90f + rotate * 360f / (alligatorLength * 6f + 6f));
        }
    }
#endif
}
