using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ControllerProximityDetector : MonoBehaviour
{

    #region Unity Events
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RemoteController"))
        {
            GameobjectsFactory.Instance.leftHandControllerMesh.SetActive(false); // Disable hand mesh
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RemoteController"))
        {
            GameobjectsFactory.Instance.leftHandControllerMesh.SetActive(true); // Enable hand mesh
        }
    }

    #endregion
}
