using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour {

    public static GameInput Instance { get; private set; }

    public event EventHandler OnSprintPerformed;
    public event EventHandler OnSprintCanceled;

    public event EventHandler OnUsePerformed;
    public event EventHandler OnUseCanceled;

    public event EventHandler OnReloadPerfomed;


    private PlayerInputActions playerInputActions;

    private void Awake() {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Sprint.performed += Sprint_performed;
        playerInputActions.Player.Sprint.canceled += Sprint_canceled;

        playerInputActions.Player.Use.performed += Use_performed;
        playerInputActions.Player.Use.canceled += Use_canceled;

        playerInputActions.Player.Reload.performed += Reload_performed;
    }

    private void Reload_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnReloadPerfomed?.Invoke(this, EventArgs.Empty);
    }

    private void Use_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnUseCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void Use_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnUsePerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Sprint_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSprintCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void Sprint_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSprintPerformed?.Invoke(this, EventArgs.Empty);
    }
}
