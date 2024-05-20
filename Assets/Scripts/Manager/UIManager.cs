using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI levelText;

    public Animator changeSceneAnimator;

    private void Awake()
    {
        Instance = this;

        levelText.text = (DataManager.GetLevel() + 1).ToString();
    }

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
}
