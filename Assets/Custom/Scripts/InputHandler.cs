using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Relays c# events from the InputSystem to other classes.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    // events
    public static event Action<InputAction.CallbackContext> InputReceived;

    // properties
    public static InputHandler Instance { get; private set; }

    public PlayerInput PlayerInput { get; private set; }

    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerInput = GetComponent<PlayerInput>();
    }

    protected void OnEnable()
    {
        foreach(InputAction action in PlayerInput.actions)
        {
            action.started += OnInput;
            action.performed += OnInput;
            action.canceled += OnInput;
        }
    }

    protected void OnDisable()
    {
        if (PlayerInput == null)
        {
            return;
        }

        foreach (InputAction action in PlayerInput.actions)
        {
            action.started -= OnInput;
            action.performed -= OnInput;
            action.canceled -= OnInput;
        }
    }

    private static void OnInput(InputAction.CallbackContext context)
    {
        InputReceived?.Invoke(context);
    }
}
