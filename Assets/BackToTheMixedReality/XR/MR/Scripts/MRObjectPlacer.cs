using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRObjectPlacer : MonoBehaviour
{

    #region Fields
    public LayerMask hitLayers;
    public Transform rayOrigin;
    public LineRenderer lineRenderer;
    public GameObject placeholderObject;

    private RaycastHit hitInfo;


    #endregion


    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        placeholderObject.SetActive(false); // Make sure the cube is initially inactive
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hitInfo, 3f, hitLayers))
        {
            // Update LineRenderer
            lineRenderer.SetPosition(0, rayOrigin.position);
            lineRenderer.SetPosition(1, hitInfo.point);

            // Position the placeholder cube at the hit point and activate it
            placeholderObject.transform.position = hitInfo.point;
            placeholderObject.SetActive(true);
        }
        else
        {
            // Extend line to a default length
            lineRenderer.SetPosition(0, rayOrigin.position);
            lineRenderer.SetPosition(1, rayOrigin.position);
            // Deactivate the placeholder cube
            placeholderObject.SetActive(false);
        }
    }

    #endregion

    #region Public Methods

    public RaycastHit GetRayHitInfo()
    {
        return hitInfo;
    }

    #endregion

}