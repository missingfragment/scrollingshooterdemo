using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class PlayerMovement : MonoBehaviour
{
    private Movement _mover;

    protected void Awake()
    {
        _mover = GetComponent<Movement>();
    }

    protected void OnEnable()
    {
        InputHandler.MoveEvent += OnMoveInput;
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 inputs = context.ReadValue<Vector2>();

        _mover.Inputs = inputs;
    }
}
