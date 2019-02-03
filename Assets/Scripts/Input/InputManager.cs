using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ButtonEvent : UnityEvent<bool>
{
}

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private IInputMethod[] _inputMethods;
    private int _currentInputMethod;

    public Vector2 RightStick => _inputMethods[_currentInputMethod].GetRightStick();
    public Vector2 LeftStick => _inputMethods[_currentInputMethod].GetLeftStick();

    public UnityEvent<bool> Primary = new ButtonEvent();
    public UnityEvent<bool> Secondary = new ButtonEvent();
    public UnityEvent<bool> Tertiary = new ButtonEvent();

    public bool CheckForSwitch = false;

    private UnityEvent<bool>[] _events;

    private bool[] _buttonsState = new bool[3];

    private void Awake()
    {
        _events = new UnityEvent<bool>[]
        {
            Primary,
            Secondary,
            Tertiary
        };
    }

    private void Start()
    {
        Component[] inputComponents = GetComponents(typeof(IInputMethod));
        var foundInputMethods = new List<IInputMethod>();

        for (int i = 0; i < inputComponents.Length; i++)
        {
            if (inputComponents[i] is IInputMethod)
            {
                foundInputMethods.Add(inputComponents[i] as IInputMethod);
            }
        }

        _inputMethods = foundInputMethods.ToArray();

        //_inputMethods = _inputMethods.Cast<IInputMethod>().ToArray();

        if (_inputMethods.Length == 0)
        {
            throw new System.Exception("[InputManager]: No input methods attached!");
        }

        Debug.Log("[InputManager]: Initialised successfully with " + _inputMethods.Length.ToString() + " input methods.");
    }

    private void Update()
    {
        bool[] newButtonsState = _inputMethods[_currentInputMethod].GetButtons();

        for (int i = 0; i < newButtonsState.Length; i++)
        {
            if (newButtonsState[i] != _buttonsState[i])
            {
                _events[i].Invoke(newButtonsState[i]);
            }
        }

        _buttonsState = newButtonsState;
    }

    private IEnumerator CheckInputSwitch()
    {
        while (true)
        {
            if (CheckForSwitch)
            {
                if (!_inputMethods[_currentInputMethod].HasInput())
                {
                    // check all other input methods for input
                    for (int i = 0; i < _inputMethods.Length; i++)
                    {
                        if (_inputMethods[i].HasInput())
                        {
                            // this method has input, switch to it!
                            _currentInputMethod = i;
                            Debug.Log("[InputManger]: switched to input method: '" + _inputMethods[i].ToString() + "'.");
                            break;
                        }
                    }
                }
            }
            // dont check on every frame
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}