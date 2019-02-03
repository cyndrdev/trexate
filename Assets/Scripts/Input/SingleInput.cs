using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Input Method for single input (keyboard only or controller only)
/// </summary>

public class SingleInput : MonoBehaviour, IInputMethod
{
    private Vector2 _leftStick;
    private Vector2 _rightStick;
    private bool[] _buttons;

    private void updateInput()
    {
        _rightStick = new Vector2(
            Input.GetAxisRaw(GameConstants.LeftHorizontal),
            Input.GetAxisRaw(GameConstants.LeftVertical)
        );
        if (_rightStick.magnitude > 1f) _rightStick.Normalize();

        _leftStick = new Vector2(
            Input.GetAxisRaw(GameConstants.RightHorizontal),
            Input.GetAxisRaw(GameConstants.RightVertical)
        );
        if (_leftStick.magnitude > 1f) _leftStick.Normalize();

        _buttons = new bool[3]
        {
            _rightStick != Vector2.zero,
            false,
            false
        };
    }

    public bool HasInput()
    {
        return (
            _leftStick == Vector2.zero &&
            _rightStick == Vector2.zero &&
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
