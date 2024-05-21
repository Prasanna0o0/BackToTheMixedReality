using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

public class AddressablePrefabLoader
{
    private GameObject prefabTemplate;
    private GameObject spawnedObject;

    public async UniTask<bool> LoadPrefabAsync(AssetReference assetReference)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetReference);

        // Await until the asset is loaded
        await handle.ToUniTask();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            prefabTemplate = handle.Result;
            Debug.Log("Prefab loaded successfully.");
            return true;
        }
        else
        {
            Debug.LogError("Failed to load prefab.");
            return false;
        }
    }

    public GameObject InstantiatePrefab(Transform parentTransform)
    {
        if (prefabTemplate != null)
        {
            spawnedObject = Object.Instantiate(prefabTemplate,parentTransform);
            Debug.Log("Prefab instantiated successfully.");
            return spawnedObject;
        }
        else
        {
            Debug.LogError("Prefab template is null. Make sure to load the prefab first.");
            return null;
        }
    }

    public GameObject InstantiatePrefab(Vector3 spawnPos)
    {
        if (prefabTemplate != null)
        {
            spawnedObject = Object.Instantiate(prefabTemplate, spawnPos,Quaternion.identity);
            Debug.Log("Prefab instantiated successfully.");
            return spawnedObject;
        }
        else
        {
            Debug.LogError("Prefab template is null. Make sure to load the prefab first.");
            return null;
        }
    }

    public void UnloadPrefab()
    {
        if (spawnedObject != null)
        {
            Object.Destroy(spawnedObject);
            spawnedObject = null;
            Debug.Log("Spawned prefab instance destroyed.");
        }

        if (prefabTemplate != null)
        {
            Addressables.Release(prefabTemplate);
            prefabTemplate = null;
            Debug.Log("Prefab template unloaded from memory.");
        }
    }
}

