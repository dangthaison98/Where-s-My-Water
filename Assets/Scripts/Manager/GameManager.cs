using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Addressables.InstantiateAsync("Level " + DataManager.GetLevel()).Completed += InitLevel;
    }

    private void InitLevel(AsyncOperationHandle<GameObject> handle)
    {
        
    }
}
