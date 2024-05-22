using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxLevel;

    private void Awake()
    {
        Instance = this;

        SpawnEasyLevel();
    }

    #region SpawnEasyLevel
    private GameObject easyLevel;
    private void SpawnEasyLevel()
    {
        int currentlevel = DataManager.GetLevel() % maxLevel + 1;
        Addressables.InstantiateAsync("Level " + currentlevel + ".1").Completed += InitLevel;
    }
    private void InitLevel(AsyncOperationHandle<GameObject> handle)
    {
        easyLevel = handle.Result;

        UIManager.Instance.changeSceneAnimator.enabled = true;
    }
    #endregion

    #region SpawnHardLevel
    [HideInInspector] public bool isHard;
    public void ActiveSpawnHardLevel()
    {
        isHard = true;
        StartCoroutine(SpawnHardLevel());
    }
    IEnumerator SpawnHardLevel()
    {
        UIManager.Instance.changeSceneAnimator.gameObject.SetActive(true);
        UIManager.Instance.changeSceneAnimator.SetTrigger("Change");
        yield return new WaitForSeconds(0.75f);
        Destroy(easyLevel);
        int currentlevel = DataManager.GetLevel() % maxLevel + 1;
        Addressables.InstantiateAsync("Level " + currentlevel + ".2").Completed += InitHardLevel;
    }
    private void InitHardLevel(AsyncOperationHandle<GameObject> handle)
    {
        UIManager.Instance.changeSceneAnimator.SetTrigger("Change");
    }
    #endregion
}
