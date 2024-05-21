using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    #region References
    [SerializeField] GameObject appLogoGameobject;
    [SerializeField] GameObject lightningGameobject;

    public Transform userCamera; // Assign the main camera (user's head) here
    public float distanceInFront = 2.0f; // How far in front of the camera the UI should appear
    public float height = 1.5f;
    [SerializeField] private float rotationSpeed = 2.5f;

    #endregion


    #region Unity Events


    private void Start()
    {
        // Check if the camera is assigned
        if (userCamera == null)
        {
            Debug.LogError("User camera not assigned!");
            return;
        }

        // Set the position of the UI canvas
        transform.position = userCamera.position + userCamera.forward * distanceInFront + new Vector3(0, height, 0);

    }

    private void Update()
    {
        // Calculate the rotation so the UI always faces the user
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - userCamera.position);
        

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    #endregion


    #region GUI Events
    public void ToggleAppSplashScreen(float fadeInDuration)
    {
        appLogoGameobject.SetActive(true);
        appLogoGameobject.GetComponentInChildren<Image>().DOFade(1f, fadeInDuration)
            .OnComplete(() => OnComplete(fadeInDuration) 
            ); 
    }

    void OnComplete(float fadeInDuration)
    {
        appLogoGameobject.GetComponent<Image>().DOFade(0f, fadeInDuration/2)
            .OnComplete(() => Destroy(lightningGameobject));

        lightningGameobject.SetActive(true);

    }
    #endregion
}
