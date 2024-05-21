using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public static StartManager instance;

    [Title("MainUI")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI ticketText;

    public TextMeshProUGUI[] levelText;
    public GameObject[] levelHolders;

    [Title("Change Scene")]
    public Animator changeSceneAnimator;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CurrencyManager.Instance.OnUpdateCurrency += UpdateCoin;
        CurrencyManager.Instance.UpdateCurrency();

        if (DataManager.GetLevel() % 2 == 0)
        {
            for (int i = 0; i < levelText.Length; i++)
            {
                levelText[i].text = (DataManager.GetLevel() + i).ToString();
            }
            levelHolders[2].SetActive(false);
            levelHolders[3].SetActive(true);
        }
        else
        {
            for (int i = 0; i < levelText.Length; i++)
            {
                levelText[i].text = (DataManager.GetLevel() + 1 + i).ToString();
            }
            levelHolders[0].SetActive(false);
            levelHolders[1].SetActive(true);
        }
    }




    public void UpdateCoin()
    {
        coinText.text = DataManager.GetCoin().ToString() + " <sprite=0>";
        ticketText.text = DataManager.GetTicket().ToString() + " <sprite=0>";
    }



    public void ActiveChangeScene(int scene)
    {
        StartCoroutine(ChangeScene(scene));
    }
    IEnumerator ChangeScene(int level)
    {
        changeSceneAnimator.gameObject.SetActive(true);
        changeSceneAnimator.SetTrigger("Change");
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(level);
    }
}
