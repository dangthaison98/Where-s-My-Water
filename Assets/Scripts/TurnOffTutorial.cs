using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffTutorial : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !isClick)
        {
            gameObject.SetActive(false);
        }
        else if(Input.GetMouseButtonUp(0) && isClick)
        {
            isClick = false;
        }
    }

    public bool isClick;
    private void OnMouseDown()
    {
        isClick = true;
    }
}
