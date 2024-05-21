using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parenter : MonoBehaviour
{

    #region Fields
    [SerializeField] GameObject parent;
    [SerializeField] Vector3 offsetPos;
    [SerializeField] Quaternion offsetRot;


    #endregion

    #region Unity Events
    private void Start()
    {
        transform.SetParent(parent.transform);
        transform.localPosition = offsetPos;
        transform.localRotation = offsetRot;
    }
    #endregion


    void AttachToParent(GameObject parent)
    {

    }


}
