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

        int currentlevel = DataManager.GetLevel() % maxLevel + 1;

        Addressables.InstantiateAsync("Level " + currentlevel).Completed += InitLevel;
    }

    private void InitLevel(AsyncOperationHandle<GameObject> handle)
    {
        
    }
}
