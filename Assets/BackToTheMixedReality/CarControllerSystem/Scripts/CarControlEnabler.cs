using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControlEnabler : MonoBehaviour
{
    private Rigidbody rb;
    float initialMass;
    float initalDrag;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initalDrag = rb.drag;
        initialMass = rb.mass;
    }

    private void OnEnable()
    {
        GlobalEventsManager.Instance.onTimeDestinationAdjustmentActived += OnTimeDestinationAdjustmentActivated;
        GlobalEventsManager.Instance.onTimeDestinationAdjustmentDeActived += OnTimeDestinationAdjustmentDeActivated;

    }

    private void OnDisable()
    {
        GlobalEventsManager.Instance.onTimeDestinationAdjustmentActived -= OnTimeDestinationAdjustmentActivated;
        GlobalEventsManager.Instance.onTimeDestinationAdjustmentDeActived -= OnTimeDestinationAdjustmentDeActivated;
    }


    void OnTimeDestinationAdjustmentActivated()
    {
        TimeMachineFactory.Instance.inputProvider.enabled = false;
        TimeMachineFactory.Instance.carControllerScript.enabled = false;
        rb.mass = 100000f;
        rb.drag = 6;
        TimeMachineFactory.Instance.carEngineSoundManager.StopCarEngineSounds();
    }
    void OnTimeDestinationAdjustmentDeActivated()
    {
        TimeMachineFactory.Instance.inputProvider.enabled = true;

        TimeMachineFactory.Instance.carControllerScript.enabled = true;
        rb.mass = initialMass;
        rb.drag = initalDrag;
        TimeMachineFactory.Instance.carEngineSoundManager.StartCarEngineSounds();
       
    }

    public void EnableCarControl()
    {
        TimeMachineFactory.Instance.carControllerScript.enabled = true;
        rb.mass = initialMass;
        rb.drag = initalDrag;
    }

    public void DisableCarControl() 
    {
    
        TimeMachineFactory.Instance.carControllerScript.enabled = false;
        rb.mass = 100000f;
        rb.drag = 6;

    }

}
