using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class ExampleScript : MonoBehaviour
{

    #region Fields

    // Assign in Editor
    public AssetReference reference;

    private GameObject loadedPrefab;

    // The addressable key for the scene to load
    public string addressableSceneKey;

    // Async operation handle for tracking the scene load operation
    private AsyncOperationHandle<SceneInstance> sceneLoadHandle;



    #endregion


    #region Prefab Loading

    [Button]
    // Method to load the Addressable prefab
    public async UniTask LoadPrefabAsync()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(reference);

        // Await until the asset is loaded
        await handle.ToUniTask();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            loadedPrefab = Instantiate(handle.Result);
            Debug.Log("Prefab loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load prefab.");
        }
    }

    [Button]

    // Method to unload the Addressable prefab
    public void UnloadPrefab()
    {
        if (loadedPrefab != null)
        {
            Destroy(loadedPrefab);
            ReleasePrefab();
            loadedPrefab = null;
            Debug.Log("Prefab unloaded successfully.");
        }
    }


    // Method to unload the Addressable prefab
    private void ReleasePrefab()
    {
        if (loadedPrefab != null)
        {
            reference.ReleaseAsset();
        }
    }

    #endregion


    #region Scene Loading


    [Button]
    // Method to load the Addressable scene asynchronously
    public async UniTask LoadSceneAsync()
    {
        // Load the scene asynchronously and store the handle
        sceneLoadHandle = Addressables.LoadSceneAsync(addressableSceneKey, LoadSceneMode.Additive);

        // Await until the scene is fully loaded
        await sceneLoadHandle.ToUniTask();

        if (sceneLoadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Scene loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load scene.");
        }
    }

    [Button]
    // Method to unload the Addressable scene
    public async UniTask UnloadSceneAsync()
    {
        if (sceneLoadHandle.IsValid())
        {
            // Unload the scene asynchronously
            await Addressables.UnloadSceneAsync(sceneLoadHandle, true).ToUniTask();

            Debug.Log("Scene unloaded successfully.");
        }
    }


    #endregion
}
