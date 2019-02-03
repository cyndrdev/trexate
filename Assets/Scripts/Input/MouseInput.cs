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

    [SerializeField]
    private Transform _playerTransform;
    
    private void updateInput()
    {
        _leftStick = new Vector2(
            Input.GetAxis(GameConstants.LeftHorizontal),
            Input.GetAxis(GameConstants.LeftVertical)
        ).normalized;

        Vector2 mousePos = Camera.main.ViewportToWorldPoint(Input.mousePosition);
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