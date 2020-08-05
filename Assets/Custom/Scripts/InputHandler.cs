using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    public static event Action<InputAction.CallbackContext> MoveEvent;
    public static event Action<InputAction.CallbackContext> FireEvent;

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
        var moveAction = PlayerInput.actions["Move"];
        moveAction.started += OnMove;
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        var fireAction = PlayerInput.actions["Fire"];
        fireAction.started += OnFire;
        fireAction.performed += OnFire;
        fireAction.canceled += OnFire;
    }

    protected void OnDisable()
    {
        if (PlayerInput == null)
        {
            return;
        }

        var moveAction = PlayerInput.actions["Move"];
        moveAction.started -= OnMove;
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        var fireAction = PlayerInput.actions["Fire"];
        fireAction.started -= OnFire;
        fireAction.performed -= OnFire;
        fireAction.canceled -= OnFire;
    }

    private static void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context);
    }

    private static void OnFire(InputAction.CallbackContext context)
    {
        FireEvent?.Invoke(context);
    }
}
