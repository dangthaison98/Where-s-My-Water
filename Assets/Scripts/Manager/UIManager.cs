using PrimeTween;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Title("Main UI")]
    public TextMeshProUGUI levelText;

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

    public void CompleteLevel()
    {
        StartCoroutine(PlayCompleteLevel());
    }
    IEnumerator PlayCompleteLevel()
    {
        fireworkEffect.SetActive(true);
        SoundManager.instance.PlayFireworkSound();
        yield return new WaitForSeconds(1.5f);
        SoundManager.instance.PlayWinSound();
        completePanel.SetActive(true);
        DataManager.EarnCoin(baseCoin);
    }

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
                baseCoin *= 3;
                DataManager.EarnCoin(baseCoin);
                gainCoinText.text = "Level reward: " + baseCoin.ToString() + "  <sprite=0>";
            }
            else if (arrowReward.anchoredPosition.x < 210 && arrowReward.anchoredPosition.x > -210)
            {
                baseCoin *= 2;
                DataManager.EarnCoin(baseCoin);
                gainCoinText.text = "Level reward: " + baseCoin.ToString() + "  <sprite=0>";
            }
            else
            {
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
