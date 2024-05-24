using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookControl : MonoBehaviour
{
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public GameObject target;

    public void ActiveCatchAnimal()
    {
        StartCoroutine(CatchAnimal());
    }
    IEnumerator CatchAnimal()
    {
        while(transform.position != target.transform.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 10 * Time.deltaTime);
            yield return null;
        }
        target.transform.parent = transform;
        while (transform.position != startPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos, 10 * Time.deltaTime);
            yield return null;
        }
        Destroy(target);
        LevelControl.Instance.CheckCompleteLevel();
    }
}
