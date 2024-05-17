using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalRotate : MonoBehaviour
{

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



    [Range(0, 10)]
    public int animalLength;
    public CapsuleCollider2D capsuleCollider;
    public GameObject head;
    public GameObject body;
    [Button(ButtonHeight = 100)]
    public void BakeAnimal()
    {
        head.transform.localPosition = new Vector3(0, 0.3f + 0.8659766f * animalLength, 0);
        if(animalLength == 0)
        {
            body.SetActive(false);
        }
        else
        {
            body.SetActive(true);
            body.transform.position = new Vector3(0, (0.8659766f * animalLength)/2 + 0.3f, 0);
            body.transform.localScale = new Vector3(0.8659766f * 4 * animalLength, 1, 1);
        }

        capsuleCollider.offset = new Vector2 (0, 0.8659766f * (animalLength + 1)/2);
        capsuleCollider.size = new Vector2 (0.5f, 0.8659766f * (animalLength + 1) + 0.5f);
    }
}
