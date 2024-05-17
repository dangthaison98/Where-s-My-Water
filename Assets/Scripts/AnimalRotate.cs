using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimalRotate : MonoBehaviour
{
    [Range(0, 9)]
    public int animalLength;

    public Transform checkPoint;
    public CapsuleCollider2D capsuleCollider;
    public GameObject head;
    public GameObject body;

    private Vector2[] mainPos = new Vector2[6];
    private Vector3 headPos = new Vector2(-0.5659766f, 0);

    private void Start()
    {
        if (animalLength == 0) return;
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

        if (animalLength == 0) return;

        //Resize Animal
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

        head.transform.localPosition = checkPoint.localPosition + headPos;
        body.transform.localScale = new Vector3((head.transform.localPosition.x - 0.3f) * 4, 1, 1);

        capsuleCollider.offset = new Vector2((head.transform.localPosition.x + 0.5659766f) / 2, 0);
        capsuleCollider.size = new Vector2(head.transform.localPosition.x + 1.0659766f, 0.5f);
    }
    private void OnMouseUp()
    {
        
    }


#if UNITY_EDITOR
    [Title("Editor"), Space(100)]
    [Min(0)]
    public int rotate;
    public Tilemap tilemap;

    [Button(ButtonHeight = 100)]
    private void BakeAnimal()
    {
        transform.position = tilemap.CellToWorld(tilemap.WorldToCell(transform.position));

        if (animalLength != 0)
        {
            mainPos[0] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), -30);
            mainPos[1] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), 30);
            mainPos[2] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), 90);
            mainPos[3] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), 150);
            mainPos[4] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), 210);
            mainPos[5] = Caculate.FindPointInCircle(transform.position, 0.8659766f * (animalLength + 1), 270);
        }

        

        float rotZ1 = 90f + Mathf.Clamp(rotate, 0, (animalLength * 6f + 6f) - 1) * 360f / (animalLength * 6f + 6f);
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

        Vector3 difference = tilemap.CellToWorld(tilemap.WorldToCell(checkPoint.position))- tilemap.CellToWorld(tilemap.WorldToCell(transform.position));
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        head.transform.localPosition = checkPoint.localPosition + headPos;
        if (animalLength == 0)
        {
            body.SetActive(false);
        }
        else
        {
            body.SetActive(true);
            body.transform.localScale = new Vector3((head.transform.localPosition.x - 0.3f) * 4, 1, 1);
        }

        capsuleCollider.offset = new Vector2((head.transform.localPosition.x + 0.5659766f) / 2, 0);
        capsuleCollider.size = new Vector2(head.transform.localPosition.x + 1.0659766f, 0.5f);
    }
#endif
}
