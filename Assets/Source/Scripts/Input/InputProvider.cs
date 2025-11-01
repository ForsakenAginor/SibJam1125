using System;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerInput
{
    public bool IsSprinted { get; }

    public event Action OnInteract;
    public event Action<Vector2> OnLook;

    public Vector2 GetMoveInput();
}

public interface IInputStateManager
{
    public event Action EscPerformed;

    public void ToMenuState();

    public void ToWorldState();
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
        _inputActions.Player.Interact.performed += OnInteractPerformed;
    }

    ~InputProvider()
    {
        _inputActions.Menu.Esc.started -= OnEscPerformed;

        _inputActions.Player.Sprint.started -= OnSprintStarted;
        _inputActions.Player.Sprint.canceled -= OnSprintCanceled;
        _inputActions.Player.Look.performed -= OnLookPerformed;
        _inputActions.Player.Interact.performed -= OnInteractPerformed;

        _inputActions.Disable();
        _inputActions.Dispose();
    }

    public event Action EscPerformed;
    public event Action<Vector2> OnLook;
    public event Action OnInteract;

    public Vector2 GetMoveInput() => _inputActions.Player.Move.ReadValue<Vector2>();

    public bool IsSprinted { get; private set; }

    public void ToWorldState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _inputActions.Player.Enable();
    }

    public void ToMenuState()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _inputActions.Player.Disable();
    }

    private void OnEscPerformed(InputAction.CallbackContext context)
    {
        EscPerformed?.Invoke();
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

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        OnInteract?.Invoke();
    }

    private enum InputState
    {
        InWorld,
        InMenu,
    }
}
