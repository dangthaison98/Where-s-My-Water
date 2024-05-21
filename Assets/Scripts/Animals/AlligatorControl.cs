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

        ReturnIdle();

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
        rb.constraints = RigidbodyConstraints2D.FreezePosition;

        CancelInvoke();
        isColli = false;
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

            SoundManager.instance.PlayAlligatorRunSound();

            StartCoroutine(RunOut());
            return;
        }
        ////////////////////////////////////////////////
        ReturnIdle();
        bodyRenderer.sprite = normalBody;

        gameObject.layer = 3;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
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

    bool isColli;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.layer == 3 && !isColli)
        {
            isColli = true;
            bodyRenderer.sprite = redBody;
            headAnim.AnimationName = "run4";
            tailAnim.AnimationName = "run4";

            SoundManager.instance.PlayAlligatorColliSound();

            Invoke(nameof(ReturnIdle), 0.5f);
        }
    }
    void ReturnIdle()
    {
        int randomNum = UnityEngine.Random.Range(0, 2);
        bodyRenderer.sprite = normalBody;
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
        isColli = false;
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
        if ((transform.eulerAngles.z >= 0 && transform.eulerAngles.z < 30) || (transform.eulerAngles.z >= 330 && transform.eulerAngles.z < 360))
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[0], mainPos[1]);
        }
        else if (transform.eulerAngles.z >= 30 && transform.eulerAngles.z < 90)
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[1], mainPos[2]);
        }
        else if (transform.eulerAngles.z >= 90 && transform.eulerAngles.z < 150)
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[2], mainPos[3]);
        }
        else if ((transform.eulerAngles.z >= 150 && transform.eulerAngles.z < 210))
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[3], mainPos[4]);
        }
        else if (transform.eulerAngles.z >= 210 && transform.eulerAngles.z < 270)
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
        capsuleCollider.size = new Vector2(head.transform.localPosition.x + 1.1659766f, 0.6f);
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
            capsuleCollider.size = new Vector2(head.transform.localPosition.x + 1.1659766f, 0.6f);
        }
        else
        {
            head.transform.localPosition = new Vector3(0.3f + 0.8659766f * alligatorLength, 0);
            checkPoint.transform.localPosition = new Vector3(0.8659766f * (alligatorLength + 1), 0);
            body.SetActive(false);

            capsuleCollider.offset = new Vector2(0.8659766f * (alligatorLength + 1) / 2, 0);
            capsuleCollider.size = new Vector2(0.8659766f * (alligatorLength + 1) + 0.6f, 0.6f);


            transform.rotation = Quaternion.Euler(0, 0, 90f + rotate * 360f / (alligatorLength * 6f + 6f));
        }
    }
#endif
}
