using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectsFactory : MonoBehaviour
{
    #region References
    public GameObject leftHandControllerAnchor;
    public GameObject rightHandControllerAnchor;
    public GameObject leftHandControllerMesh;
    public GameObject rightHandControllerMesh;



    #endregion


    #region Singleton Implementation

    public static GameobjectsFactory Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

}
