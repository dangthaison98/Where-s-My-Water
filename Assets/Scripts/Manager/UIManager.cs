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

    [Title("Change Scene")]
    public Animator changeSceneAnimator;

    private void Awake()
    {
        Instance = this;

        levelText.text = (DataManager.GetLevel() + 1).ToString();
    }

    public void CompleteLevel()
    {
        StartCoroutine(PlayCompleteLevel());
    }
    IEnumerator PlayCompleteLevel()
    {
        fireworkEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        completePanel.SetActive(true);
    }

    #region Button
    public void NextLevelButton()
    {
        ActiveChangeScene(SceneManager.GetActiveScene().buildIndex);
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
