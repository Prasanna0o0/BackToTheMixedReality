using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScenePermissionsManager : MonoBehaviour
{

    public TextMeshProUGUI testText;


    private void OnEnable()
    {
        //SubscribeVRFocusEvents();
    }

    private void OnDisable()
    {
       // UnSubscribeVRFocusEvents();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            CheckScenePermissions();
        }
    }

   

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        CheckScenePermissions();
    }

    void CheckScenePermissions()
    {
        const string spatialPermission = "com.oculus.permission.USE_SCENE";
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(spatialPermission))
        {
            var callbacks = new UnityEngine.Android.PermissionCallbacks();
            callbacks.PermissionDenied += Denied;
            callbacks.PermissionGranted += Granted;
            testText.text = "Denied.";

            // avoid callbacks.PermissionDeniedAndDontAskAgain. PermissionDenied is
            // called instead unless you subscribe to PermissionDeniedAndDontAskAgain.

            UnityEngine.Android.Permission.RequestUserPermission(spatialPermission, callbacks);
        }
        else
        {
            testText.text = "Granted.";

        }
    }

    void Denied(string permission)
    {
        Debug.Log($"{permission} Denied");
        testText.text = "Denied.";

    }
    void Granted(string permission)
    {
        testText.text = "Granted.";


        Debug.Log($"{permission} Granted");
    }




    public void SubscribeVRFocusEvents()
    {
        OVRManager.VrFocusLost += OnVRFocusLost;
        OVRManager.VrFocusAcquired += OnVRFocusAcquired;
    }

    public void UnSubscribeVRFocusEvents()
    {
        OVRManager.VrFocusLost -= OnVRFocusLost;
        OVRManager.VrFocusAcquired -= OnVRFocusAcquired;
    }

    private void OnVRFocusAcquired()
    {
        CheckScenePermissions();
        Debug.Log("Switch to VR camera");
    }

    public void OnVRFocusLost()
    {
        if (OVRManager.hasVrFocus == false)
        {



        }
    }

    


}
