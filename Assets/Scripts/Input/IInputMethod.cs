using UnityEngine;
using UnityEngine.Events;

public interface IInputMethod
{
    bool HasInput();
    Vector2 GetLeftStick();
    Vector2 GetRightStick();
    bool[] GetButtons();
}
