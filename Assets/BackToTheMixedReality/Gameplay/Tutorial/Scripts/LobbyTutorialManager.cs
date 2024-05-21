using Phantom.Environment.Scripts;
using PhantoUtils.VR;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class LobbyTutorialManager : MonoBehaviour
{
    GameInputActions inputActions;

    [SerializeField]
    [Tooltip(
        "This is set automatically when sceneToLoad is set. If the scene name changes or this name is incorrect, update the value of sceneToLoad.")]
    private string _sceneNameToLoad;

    [SerializeField] private LoadSceneMode loadMode = LoadSceneMode.Single;
    [SerializeField] private GameObject panel_PermissionNotGrantedDoNotAskAgain;
    [SerializeField] private GameObject panel_PermissionRequired;
    [SerializeField] private GameObject panel_NoSceneModelAvailable;
    [SerializeField] private GameObject panel_RoomSetupPermission;

    [SerializeField] private GameObject panel_SceneModelAvailable;
    [SerializeField] private GameObject panel_GameInfo;


    // Permission string for requesting scene data permission
    private static readonly string SCENE_PERMISSION = "com.oculus.permission.USE_SCENE";
    const string spatialPermission = "com.oculus.permission.USE_SCENE";


    private SceneDataLoader _dataLoader;
    [SerializeField] private GameObject SceneApiDataLoaderReference;

    private bool _permissionGranted = false;

    #region Unity Events
    private void Start()
    {
        SceneApiDataLoaderReference.SetActive(false); // Turn off scene related stuff until permission is granted                                            
        ActivatePanel(string.Empty);
        DeActivateGameInfoPanel();
        CheckScenePermissions();


    }

    private void OnEnable()
    {
        inputActions = new GameInputActions();
        inputActions.Enable();

    }

    private void OnDisable()
    {
        inputActions.Disable();

    }

    private void OnDestroy()
    {
        inputActions.LobbyTutorialAM.LeftTrigger.performed -= _ => StartRoomSetup();
        inputActions.LobbyTutorialAM.RightTrigger.performed -= _ => LoadGameScene();


    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus && !_permissionGranted)
        {
            CheckScenePermissions();
        }
    }

    #endregion

    void CheckScenePermissions()
    {

        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(spatialPermission))
        {
            //Showing no panels
            //ActivatePanel(string.Empty);

            var callbacks = new UnityEngine.Android.PermissionCallbacks();
            callbacks.PermissionDenied += Denied;
            callbacks.PermissionGranted += Granted;

            // avoid callbacks.PermissionDeniedAndDontAskAgain. PermissionDenied is
            // called instead unless you subscribe to PermissionDeniedAndDontAskAgain.
            //Request permission
            UnityEngine.Android.Permission.RequestUserPermission(spatialPermission, callbacks);
        }
        else
        {
            //It means the permission granted.
            ActOnPermissionGranted();

        }

    }

    void Denied(string permission)
    {
        Debug.Log($"{permission} Denied");
        ActOnPermissionDenied();


    }
    void Granted(string permission)
    {
        _permissionGranted = true;
        ActOnPermissionGranted();
        Debug.Log($"{permission} Granted");
    }


    /// <summary>
    /// Show lobby when permission is granted
    /// </summary>
    private void ActOnPermissionGranted()
    {
        ActivatePanel(panel_RoomSetupPermission.name);
        inputActions.LobbyTutorialAM.LeftTrigger.performed += _ => StartRoomSetup();
        DeActivateGameInfoPanel();

    }

    /// <summary>
    /// Handle denied permission states
    /// </summary>
    /// <param name="dontAskAgain">Whether the user has clicked the 'DontAskAgain' checkbox on the permission dialog</param>
    private void ActOnPermissionDenied()
    {
        ActivatePanel(panel_PermissionNotGrantedDoNotAskAgain.name);
    }

    void StartRoomSetup()
    {
        SceneApiDataLoaderReference.SetActive(true);

    }

    public void OnNoSceneModelAvailable()
    {
        ActivatePanel(panel_NoSceneModelAvailable.name);
    }

    public void OnSceneModelAvailable()
    {       
        ActivateGameInfoPanel();
        ActivatePanel(panel_SceneModelAvailable.name);
        inputActions.LobbyTutorialAM.LeftTrigger.performed += _ => ReScanRoom();
        inputActions.LobbyTutorialAM.RightTrigger.performed += _ => LoadGameScene();
    }

    void ReScanRoom()
    {
        SceneApiDataLoaderReference.GetComponent<SceneDataLoader>().Rescan();
    }




    void ActivatePanel(string panelName)
    {
        panel_NoSceneModelAvailable.SetActive(panel_NoSceneModelAvailable.gameObject.name.Equals(panelName));
        panel_SceneModelAvailable.SetActive(panel_SceneModelAvailable.gameObject.name.Equals(panelName));
        panel_PermissionRequired.SetActive(panel_PermissionRequired.gameObject.name.Equals(panelName));
        panel_PermissionNotGrantedDoNotAskAgain.SetActive(panel_PermissionNotGrantedDoNotAskAgain.gameObject.name.Equals(panelName));
        panel_RoomSetupPermission.SetActive(panel_RoomSetupPermission.name.Equals(panelName));
    }

    void ActivateGameInfoPanel()
    {
        panel_GameInfo.SetActive(true);

    }

    void DeActivateGameInfoPanel()
    {
        panel_GameInfo.SetActive(false);

    }


    #region Scene Loading
    public void LoadGameScene()
    {
        LoadSceneAsync();
    }
    public void UnloadScene()
    {
        UnloadSceneAsync();
    }

    AsyncOperation LoadSceneAsync()
    {
        return SceneManager.LoadSceneAsync(_sceneNameToLoad, loadMode);
    }

    AsyncOperation UnloadSceneAsync()
    {
        return SceneManager.UnloadSceneAsync(_sceneNameToLoad);
    }

    #endregion
}
