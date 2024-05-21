using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TimeMachineSpawner : MonoBehaviour
{
    #region Fields
    public AssetReference timeMachineAssetReference;
    private GameObject spawnedTimeMachineObject;

    AddressablePrefabLoader prefabLoader;


    private Vector3 originalPos;
    GameInputActions inputActions;


    public UnityEvent onObjectSpawned;
    #endregion

    #region Unity Events

    private void Awake()
    {
        inputActions = new GameInputActions();

        // Register the event for the SpawnObject action
        inputActions.GameAM.SpawnObject.performed += _ => SpawnTimeMachine();
        //inputActions.GameAM.ResetCarTransform.performed += _ => ResetCar();
        prefabLoader = new AddressablePrefabLoader();


    }

    private void OnEnable()
    {
        // Enable the input actions
        inputActions.Enable();
    }

    private void OnDisable()
    {
        // Disable the input actions
        inputActions.Disable();
    }

    #endregion

    public async UniTask LoadIntoMemory()
    {
        await prefabLoader.LoadPrefabAsync(timeMachineAssetReference);
    }

   
    private void SpawnTimeMachine()
    {
        if (spawnedTimeMachineObject==null)
        {
            //Getting spawn position
            Vector3 spawnPos =
                GameScriptsFactory.Instance.mrObjectPlacerScript.GetRayHitInfo().point + new Vector3(0f,0.3f,0f);
            originalPos = spawnPos;

            // Instantiate the TimeMachine prefab at the current position
            spawnedTimeMachineObject = prefabLoader.InstantiatePrefab(spawnPos);
            onObjectSpawned.Invoke();
            DOVirtual.DelayedCall(1.5f, () => TimeMachineFactory.Instance.timeTravelManager.canTimeTravel = true);
            DisableSpawning();
        }

    }

    
    private void DisableSpawning()
    {
        GameScriptsFactory.Instance.mrObjectPlacerScript.gameObject.SetActive(false);
    }


  

  



}
