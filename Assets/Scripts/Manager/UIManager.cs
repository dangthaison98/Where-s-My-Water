using PrimeTween;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Title("Main UI")]
    public TextMeshProUGUI levelText;
    public GameObject mainUi;
    public TextMeshProUGUI itemTuto;
    public GameObject[] levelIcon;
    public GameObject hardLevelWarning;

    [Title("Item")]
    public GameObject[] itemButtons;

    [Title("Panel")]
    public GameObject completePanel;

    [Title("Effect")]
    public GameObject fireworkEffect;

    [Title("Reward")]
    public int baseCoin;
    public TextMeshProUGUI gainCoinText;
    public GameObject bonusButton;
    public GameObject noThankButton;
    public GameObject nextLevelButton;
    public RectTransform arrowReward;

    [Title("Change Scene")]
    public Animator changeSceneAnimator;

    private void Awake()
    {
        Instance = this;

        levelText.text = (DataManager.GetLevel() + 1).ToString();

        gainCoinText.text = "Level reward: " + baseCoin.ToString() + "  <sprite=0>";
    }

    #region Item
    [HideInInspector] public bool isUseHook;
    [HideInInspector] public bool isUseHammer;
    [HideInInspector] public int animalCountHook;
    public void ChoiceHook()
    {
        if (isUseHook)
        {
            isUseHook = false;
            for (int i = 0; i < itemButtons.Length; i++)
            {
                itemButtons[i].SetActive(true);
            }
            mainUi.SetActive(true);
            itemTuto.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            isUseHook = true;
            for (int i = 0; i < itemButtons.Length; i++)
            {
                itemButtons[i].SetActive(false);
            }
            itemButtons[0].SetActive(true);
            mainUi.SetActive(false);
            itemTuto.text = "Catch alligators or turtles";
            itemTuto.transform.parent.gameObject.SetActive(true);
        }
    }
    public void UseHook()
    {
        isUseHook = false;
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].SetActive(true);
        }
        mainUi.SetActive(true);
        itemTuto.transform.parent.gameObject.SetActive(false);

        animalCountHook--;

        if (UIManager.Instance.animalCountHook <= 0)
        {
            UIManager.Instance.itemButtons[0].GetComponent<Button>().interactable = false;
        }
        else
        {
            UIManager.Instance.itemButtons[0].GetComponent<Button>().interactable = true;
        }
    }

    public void ChoiceHammer()
    {
        if (isUseHammer)
        {
            isUseHammer = false;
            for (int i = 0; i < itemButtons.Length; i++)
            {
                itemButtons[i].SetActive(true);
            }
            mainUi.SetActive(true);
            itemTuto.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            isUseHammer = true;
            for (int i = 0; i < itemButtons.Length; i++)
            {
                itemButtons[i].SetActive(false);
            }
            itemButtons[1].SetActive(true);
            mainUi.SetActive(false);
            itemTuto.text = "Crack clams, frogs and crabs";
            itemTuto.transform.parent.gameObject.SetActive(true);
        }
    }
    [HideInInspector] public int animalCountHammer;
    public void UseHammer()
    {
        isUseHammer = false;
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].SetActive(true);
        }
        mainUi.SetActive(true);
        itemTuto.transform.parent.gameObject.SetActive(false);

        animalCountHammer--;

        if (UIManager.Instance.animalCountHammer <= 0)
        {
            UIManager.Instance.itemButtons[1].GetComponent<Button>().interactable = false;
        }
        else
        {
            UIManager.Instance.itemButtons[1].GetComponent<Button>().interactable = true;
        }
    }
    #endregion

    #region Complete Level
    public void CompleteLevel()
    {
        StartCoroutine(PlayCompleteLevel());
    }
    IEnumerator PlayCompleteLevel()
    {
        fireworkEffect.SetActive(false);
        fireworkEffect.SetActive(true);
        SoundManager.instance.PlayFireworkSound();
        yield return new WaitForSeconds(1.5f);
        SoundManager.instance.PlayWinSound();
        completePanel.SetActive(true);
        DataManager.EarnCoin(baseCoin);
    }
    #endregion

    #region Button
    public void NextLevelButton()
    {
        ActiveChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BonusReward()
    {
        if (DataManager.SpendTicket(1))
        {
            arrowReward.GetComponent<Animation>().Stop();
            if (arrowReward.anchoredPosition.x < 70 && arrowReward.anchoredPosition.x > -70)
            {
                baseCoin *= 5;
                DataManager.EarnCoin(baseCoin);
                gainCoinText.text = "Level reward: " + baseCoin.ToString() + "  <sprite=0>";
            }
            else if (arrowReward.anchoredPosition.x < 210 && arrowReward.anchoredPosition.x > -210)
            {
                baseCoin *= 3;
                DataManager.EarnCoin(baseCoin);
                gainCoinText.text = "Level reward: " + baseCoin.ToString() + "  <sprite=0>";
            }
            else
            {
                baseCoin *= 2;
                DataManager.EarnCoin(baseCoin);
                gainCoinText.text = "Level reward: " + baseCoin.ToString() + "  <sprite=0>";
            }
            //coinEffect.SetActive(true);
            bonusButton.SetActive(false);
            noThankButton.SetActive(false);
            nextLevelButton.SetActive(true);

            CurrencyManager.Instance.UpdateCurrency();
        }
        else
        {
            
        }
    }
    #endregion

    #region Change scene
    public void ActiveChangeScene(int scene)
    {
        ChangeSceneEffect(scene);
    }
    void ChangeSceneEffect(int level)
    {
        changeSceneAnimator.gameObject.SetActive(true);
        changeSceneAnimator.SetTrigger("Change");
        levelToLoad = level;
        Invoke(nameof(LoadScene), 0.75f);
    }
    private int levelToLoad;
    void LoadScene()
    {
        SceneManager.LoadScene(levelToLoad);
    }
    #endregion
}
