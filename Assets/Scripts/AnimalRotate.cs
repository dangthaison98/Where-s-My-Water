using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalRotate : MonoBehaviour
{

    public Transform checkPoint;

    private void OnMouseDown()
    {
        Debug.Log("Down");
    }
    private void OnMouseDrag()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
    }
    private void OnMouseUp()
    {
        Debug.Log("Up");
    }


#if UNITY_EDITOR
    [Range(0, 10)]
    public int animalLength;
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
