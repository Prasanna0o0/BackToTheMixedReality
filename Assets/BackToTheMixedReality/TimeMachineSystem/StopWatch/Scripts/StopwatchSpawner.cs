using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using TMPro;
using System;

public class StopwatchSpawner : MonoBehaviour
{
    #region Fields

    [Space]
    public AssetReference remoteControllerAssetReference;
    private GameObject spawnedStopWatchObject;
    AddressablePrefabLoader prefabLoader;
    bool isSpawned = false;

    #endregion


    #region Unity Events

    private void Awake()
    {
        prefabLoader = new AddressablePrefabLoader();
    }


    #endregion

    #region Spawning StopWatch Events


    public async UniTask LoadIntoMemory()
    {
        await prefabLoader.LoadPrefabAsync(remoteControllerAssetReference);
    }

    public void SpawnStopWatchPrefab()
    {
        if (!isSpawned)
        {
            spawnedStopWatchObject = prefabLoader.InstantiatePrefab(GameobjectsFactory.Instance.leftHandControllerAnchor.transform);
            spawnedStopWatchObject.transform.localPosition = Vector3.zero;
            spawnedStopWatchObject.transform.localRotation = Quaternion.identity;

        }
        else
        {
            ActivateStopWatchMesh();
        }
        DeActivateLeftHandControllerMesh();
        
    }


    void ActivateStopWatchMesh()
    {
        spawnedStopWatchObject.SetActive(true);
        DeActivateLeftHandControllerMesh();
    }

    void DeActivateStopWatchMesh()
    {
        spawnedStopWatchObject.SetActive(false);
        ActivateLeftHandControllerMesh();
    }

    public void DeActivateLeftHandControllerMesh()
    {
        if (GameobjectsFactory.Instance.leftHandControllerMesh != null)
        {
            GameobjectsFactory.Instance.leftHandControllerMesh.SetActive(false);
        }
    }

    public void ActivateLeftHandControllerMesh()
    {
        if (GameobjectsFactory.Instance.leftHandControllerMesh != null)
        {
            GameobjectsFactory.Instance.leftHandControllerMesh.SetActive(true);
        }
    }


    #endregion


  
}
