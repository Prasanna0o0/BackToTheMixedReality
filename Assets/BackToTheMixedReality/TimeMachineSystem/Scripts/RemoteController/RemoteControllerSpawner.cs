using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 1.Spawn Remote Controller when the user spawned the Time Machine
/// 2.
/// </summary>

public class RemoteControllerSpawner : MonoBehaviour
{


    #region Fields
    public AssetReference remoteControllerAssetReference;

    private GameObject spawnedRemoteController;

    AddressablePrefabLoader prefabLoader;


   
    bool isSpawned = true;
    #endregion


    #region Unity Events
    private void Awake()
    {
        prefabLoader = new AddressablePrefabLoader();
    }

    #endregion



    #region Remote Controller Events

    public async UniTask LoadIntoMemory()
    {
        await prefabLoader.LoadPrefabAsync(remoteControllerAssetReference);
    }

    public void SpawnRemoteControllerPrefab()
    {
        spawnedRemoteController = prefabLoader.InstantiatePrefab(GameobjectsFactory.Instance.rightHandControllerAnchor.transform);
        spawnedRemoteController.transform.localPosition = Vector3.zero;
        spawnedRemoteController.transform.localRotation = Quaternion.identity;


    }

    public void DeActivateRightHandControllerMesh()
    {
        if (GameobjectsFactory.Instance.rightHandControllerMesh != null)
        {
            GameobjectsFactory.Instance.rightHandControllerMesh.SetActive(false);
        }
    }

    public void ActivateRightHandControllerMesh()
    {
        if (GameobjectsFactory.Instance.rightHandControllerMesh != null)
        {
            GameobjectsFactory.Instance.rightHandControllerMesh.SetActive(true);
        }
    }

   

    public void ActivateRemoteControllerMesh()
    {
        spawnedRemoteController.SetActive(true);
    }

    public void DeActivateRemoteControllerMesh()
    {
        spawnedRemoteController.SetActive(false);

    }


    #endregion
}
