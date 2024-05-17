using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalRotate : MonoBehaviour
{
    [Range(0, 10)]
    public int animalLength;

    public Transform checkPoint;

    private Vector2[] mainPos = new Vector2[6];

    private void Awake()
    {
        mainPos[0] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), -30);
        mainPos[1] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), 30);
        mainPos[2] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), 90);
        mainPos[3] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), 150);
        mainPos[4] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), 210);
        mainPos[5] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), 270);
    }

    private void OnMouseDown()
    {
        
    }
    private void OnMouseDrag()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        if (rotZ >= -30 && rotZ < 30)
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[0], mainPos[1]);
        }
        else if (rotZ >= 30 && rotZ < 90)
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[1], mainPos[2]);
        }
        else if (rotZ >= 90 && rotZ < 150)
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[2], mainPos[3]);
        }
        else if ((rotZ >= 150 && rotZ < 180) || (rotZ >= -180 && rotZ < -150))
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[3], mainPos[4]);
        }
        else if (rotZ >= -150 && rotZ < -90)
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[4], mainPos[5]);
        }
        else
        {
            checkPoint.position = Caculate.GetIntersectionPoint(transform.position, checkPoint.position,
                mainPos[5], mainPos[0]);
        }
    }
    private void OnMouseUp()
    {
        
    }


#if UNITY_EDITOR
    [Min(0)]
    public int rotate;
    public CapsuleCollider2D capsuleCollider;
    public GameObject head;
    public GameObject body;

    private void OnValidate()
    {
        head.transform.localPosition = new Vector3(0.3f + 0.8659766f * animalLength, 0);
        checkPoint.transform.localPosition = new Vector3(0.8659766f * (animalLength + 1), 0);
        if (animalLength == 0)
        {
            body.SetActive(false);
        }
        else
        {
            body.SetActive(true);
            body.transform.localScale = new Vector3(0.8659766f * 4 * animalLength, 1, 1);
        }

        capsuleCollider.offset = new Vector2(0.8659766f * (animalLength + 1) / 2, 0);
        capsuleCollider.size = new Vector2(0.8659766f * (animalLength + 1) + 0.5f, 0.5f);


        transform.rotation = Quaternion.Euler(0, 0, 90f + rotate * 360f/(animalLength * 6f + 6f));
    }
#endif
}
