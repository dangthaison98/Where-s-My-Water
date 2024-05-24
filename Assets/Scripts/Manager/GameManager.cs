using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxLevel;

    public HookControl hook;
    public GameObject hammer;

    private void Awake()
    {
        Instance = this;
        PoolManager.ClearPool();
        SpawnEasyLevel();
    }

    public void SpawnHammer(GameObject animal)
    {
        UIManager.Instance.UseHammer();
        var hammerControl =  PoolManager.Spawn("Hammer", hammer, animal.transform.position, Quaternion.identity);
        StartCoroutine(DestroyAnimal(animal, 1.5f, hammerControl));
    }
    public void SpawnHook(GameObject animal)
    {
        UIManager.Instance.UseHook();
        Vector3 startPos = new Vector3(animal.transform.position.x, Camera.main.orthographicSize + 0.5f + Camera.main.transform.position.y, 0);
        HookControl hookControl = PoolManager.Spawn<HookControl>("Hook", hook, startPos, Quaternion.identity);
        hookControl.target = animal;
        hookControl.startPos = startPos;
        hookControl.ActiveCatchAnimal();
    }
    IEnumerator DestroyAnimal(GameObject animal, float time, GameObject hammer)
    {
        yield return new WaitForSeconds(time);
        Destroy(animal);
        yield return new WaitForSeconds(1);
        PoolManager.Despawn("Hammer", hammer);
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

        UIManager.Instance.animalCountHook = FindObjectsByType<AlligatorControl>(FindObjectsSortMode.None).Length + FindObjectsByType<TurtleControl>(FindObjectsSortMode.None).Length;
        if(UIManager.Instance.animalCountHook <= 0)
        {
            UIManager.Instance.itemButtons[0].GetComponent<Button>().interactable = false;
        }
        UIManager.Instance.animalCountHammer = FindObjectsByType<OysterControl>(FindObjectsSortMode.None).Length + FindObjectsByType<FrogControl>(FindObjectsSortMode.None).Length;
        if (UIManager.Instance.animalCountHammer <= 0)
        {
            UIManager.Instance.itemButtons[1].GetComponent<Button>().interactable = false;
        }
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
        UIManager.Instance.fireworkEffect.SetActive(true);
        SoundManager.instance.PlayFireworkSound();
        yield return new WaitForSeconds(1.5f);
        UIManager.Instance.changeSceneAnimator.gameObject.SetActive(true);
        UIManager.Instance.changeSceneAnimator.SetTrigger("Change");
        yield return new WaitForSeconds(0.75f);
        UIManager.Instance.levelIcon[0].SetActive(false);
        UIManager.Instance.levelIcon[1].SetActive(true);
        Destroy(easyLevel);
        int currentlevel = DataManager.GetLevel() % maxLevel + 1;
        Addressables.InstantiateAsync("Level " + currentlevel + ".2").Completed += InitHardLevel;
    }
    private void InitHardLevel(AsyncOperationHandle<GameObject> handle)
    {
        UIManager.Instance.animalCountHook = FindObjectsByType<AlligatorControl>(FindObjectsSortMode.None).Length + FindObjectsByType<TurtleControl>(FindObjectsSortMode.None).Length;
        if (UIManager.Instance.animalCountHook <= 0)
        {
            UIManager.Instance.itemButtons[0].GetComponent<Button>().interactable = false;
        }
        else
        {
            UIManager.Instance.itemButtons[0].GetComponent<Button>().interactable = true;
        }
        UIManager.Instance.animalCountHammer = FindObjectsByType<OysterControl>(FindObjectsSortMode.None).Length + FindObjectsByType<FrogControl>(FindObjectsSortMode.None).Length;
        if (UIManager.Instance.animalCountHammer <= 0)
        {
            UIManager.Instance.itemButtons[1].GetComponent<Button>().interactable = false;
        }
        else
        {
            UIManager.Instance.itemButtons[1].GetComponent<Button>().interactable = true;
        }


        UIManager.Instance.changeSceneAnimator.SetTrigger("Change");
        Invoke(nameof(PlayHardLevelSound), 0.5f);
    }
    void PlayHardLevelSound()
    {
        UIManager.Instance.hardLevelWarning.SetActive(true);
        SoundManager.instance.PlayWarningSound();
    }
    #endregion
}
