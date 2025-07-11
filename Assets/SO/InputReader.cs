using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputReader", menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, PlayerInput.IBlackHoleActions
{
    public event Action OnEscKeyPressed;
    public Vector2 HoleInputDirection { get; private set; }
    public Vector2 TetrisInputDirection { get; private set; }

    private PlayerInput _playerInput;

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInput();
            _playerInput.BlackHole.SetCallbacks(this);
        }
        _playerInput.BlackHole.Enable();
    }

    private void OnDisable()
    {
        _playerInput.BlackHole.Disable();
    }

    public void OnESC(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            OnEscKeyPressed?.Invoke();
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        HoleInputDirection = context.ReadValue<Vector2>();
    }

    public void OnTetris(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        TetrisInputDirection = context.ReadValue<Vector2>();
    }
}
