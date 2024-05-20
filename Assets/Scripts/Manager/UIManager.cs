using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    private void Awake()
    {
        levelText.text = (DataManager.GetLevel() + 1).ToString();
    }
}
