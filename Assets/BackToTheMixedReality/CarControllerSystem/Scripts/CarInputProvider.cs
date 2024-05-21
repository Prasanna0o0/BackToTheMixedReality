using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputProvider : MonoBehaviour, GenericCarControllerInputActions.IGenericCarControlAMActions
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public float Brake { get; private set; }

    private GenericCarControllerInputActions inputActions;

    private void Awake()
    {
        inputActions = new GenericCarControllerInputActions();

        // Set up the callback interfaces
        inputActions.GenericCarControlAM.SetCallbacks(this);
    }

    private void OnEnable()
    {
        inputActions.GenericCarControlAM.Enable();
    }

    private void OnDisable()
    {
        inputActions.GenericCarControlAM.Disable();
    }

    public void OnDrive(InputAction.CallbackContext context)
    {
        // Read the value from the Drive action
        Vertical = context.ReadValue<float>();
        
    }

    public void OnQuestDrive(InputAction.CallbackContext context)
    {
        TimeMachineFactory.Instance.carControlEnabler.EnableCarControl();

        Vertical = context.ReadValue<Vector2>().y;
    }

    public void OnSteer(InputAction.CallbackContext context)
    {
        TimeMachineFactory.Instance.carControlEnabler.EnableCarControl();

        // Read the value from the Steer action
        Horizontal = context.ReadValue<float>();

    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        // Read the value from the Brake action
        Brake = context.ReadValue<float>();
    }

   
    public void OnResetCar(InputAction.CallbackContext context)
    {
        transform.position = new Vector3(0, 0.03f, 1);
        transform.rotation = Quaternion.identity;
    }

    public void OnEnableDisableCarController(InputAction.CallbackContext context)
    {
        TimeMachineFactory.Instance.carControlEnabler.EnableCarControl();
    }

   
}
