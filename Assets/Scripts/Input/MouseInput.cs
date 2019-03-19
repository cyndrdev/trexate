using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Input Method for mouse + keyboard combo
/// </summary>

public class MouseInput : MonoBehaviour, IInputMethod
{
    private Vector2 _leftStick;
    private Vector2 _rightStick;
    private bool[] _buttons;

#pragma warning disable 0649
    [SerializeField]
    private Transform _playerTransform;
#pragma warning restore 0649
    
    private void updateInput()
    {
        _leftStick = new Vector2(
            Input.GetAxisRaw(GameConstants.LeftHorizontal),
            Input.GetAxisRaw(GameConstants.LeftVertical)
        );
        if (_leftStick.magnitude > 1f) _leftStick.Normalize();

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = _playerTransform.position;
        _rightStick = (mousePos - playerPos).normalized;

        _buttons = new bool[3]
        {
            Input.GetMouseButton(0),
            false,
            false
        };
    }

    public bool HasInput()
    {
        return (
            _leftStick == Vector2.zero &&
            !_buttons[0] &&
            !_buttons[1] &&
            !_buttons[2]
        );
    }

    public Vector2 GetLeftStick() => _leftStick;

    public Vector2 GetRightStick() => _rightStick;

    public bool[] GetButtons() => _buttons;

    void Update() => updateInput();
    void Start() => updateInput();
}