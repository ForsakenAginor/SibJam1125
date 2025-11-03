using System;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerInput
{
    public bool IsSprinted { get; }

    public bool IsCharging {  get; }

    public event Action OnClean;
    public event Action OnRadarPressed;
    public event Action OnInteractStart;
    public event Action OnInteractCancel;
    public event Action OnJump;
    public event Action<Vector2> OnLook;

    public Vector2 GetMoveInput();
}

public interface IInputStateManager
{
    public event Action EscPerformed;

    public void ToMenuState();

    public void ToWorldState();

    public void ToFinishState();
}

public class InputProvider : IPlayerInput, IInputStateManager
{
    private readonly OurInputActions _inputActions;

    public InputProvider()
    {
        _inputActions = new OurInputActions();
        _inputActions.Menu.Enable();
        _inputActions.Player.Enable();

        _inputActions.Menu.Esc.started += OnEscPerformed;

        _inputActions.Player.Sprint.started += OnSprintStarted;
        _inputActions.Player.Sprint.canceled += OnSprintCanceled;
        _inputActions.Player.Look.performed += OnLookPerformed;
        _inputActions.Player.Jump.performed += OnJumpPerformed;

        _inputActions.Player.Clean.performed += OnCleanPerformed;
        _inputActions.Player.Radar.performed += OnRadarPerformed;
        _inputActions.Player.Charge.started += OnChargeStarted;
        _inputActions.Player.Charge.canceled += OnChargeCanceled;

        _inputActions.Player.Interact.started += OnInteractStarted;
        _inputActions.Player.Interact.canceled += OnInteractCanceled;
    }

    ~InputProvider()
    {
        _inputActions.Menu.Esc.started -= OnEscPerformed;

        _inputActions.Player.Clean.performed -= OnCleanPerformed;
        _inputActions.Player.Radar.performed -= OnRadarPerformed;
        _inputActions.Player.Charge.started -= OnChargeStarted;
        _inputActions.Player.Charge.canceled -= OnChargeCanceled;

        _inputActions.Player.Sprint.started -= OnSprintStarted;
        _inputActions.Player.Sprint.canceled -= OnSprintCanceled;
        _inputActions.Player.Look.performed -= OnLookPerformed;
        _inputActions.Player.Interact.started -= OnInteractStarted;
        _inputActions.Player.Interact.canceled -= OnInteractCanceled;
        _inputActions.Player.Jump.performed -= OnJumpPerformed;

        _inputActions.Disable();
        _inputActions.Dispose();
    }

    public event Action EscPerformed;
    public event Action<Vector2> OnLook;
    public event Action OnInteractStart;
    public event Action OnInteractCancel;
    public event Action OnJump;

    public event Action OnClean;
    public event Action OnRadarPressed;

    public Vector2 GetMoveInput() => _inputActions.Player.Move.ReadValue<Vector2>();

    public bool IsCharging { get; private set; }

    public bool IsSprinted { get; private set; }

    public void ToWorldState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _inputActions.Player.Enable();
        _inputActions.Menu.Enable();
    }

    public void ToMenuState()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _inputActions.Player.Disable();
        _inputActions.Menu.Enable();
    }

    public void ToFinishState()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _inputActions.Player.Disable();
        _inputActions.Menu.Disable();
    }

    private void OnEscPerformed(InputAction.CallbackContext context)
    {
        EscPerformed?.Invoke();
    }

    private void OnCleanPerformed(InputAction.CallbackContext context)
    {
        OnClean?.Invoke();
    }

    private void OnRadarPerformed(InputAction.CallbackContext context)
    {
        OnRadarPressed?.Invoke();
    }

    private void OnChargeStarted(InputAction.CallbackContext context)
    {
        IsCharging = true;
    }

    private void OnChargeCanceled(InputAction.CallbackContext context)
    {
        IsCharging = false;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }

    private void OnSprintStarted(InputAction.CallbackContext context)
    {
        IsSprinted = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        IsSprinted = false;
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        OnLook?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        OnInteractCancel?.Invoke();
    }

    private void OnInteractStarted(InputAction.CallbackContext context)
    {
        OnInteractStart?.Invoke();
    }

    private enum InputState
    {
        InWorld,
        InMenu,
    }
}
