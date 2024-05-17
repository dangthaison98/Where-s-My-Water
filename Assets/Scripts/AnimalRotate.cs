using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalRotate : MonoBehaviour
{
    public int animalLength;

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
}
