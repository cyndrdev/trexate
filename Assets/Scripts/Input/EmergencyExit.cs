using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyExit : MonoBehaviour
{
    private float _time = 0;
    private KeyCode _exit = KeyCode.Escape;

    void Update()
    {
        if (Input.GetKeyUp(_exit))
        {
            _time = 0;
            Game.Instance.TogglePause();
        }

        if (Input.GetKey(_exit))
        {
            _time += Time.unscaledDeltaTime;

            if (_time > GameConstants.EmergencyExitDuration)
            {
                Application.Quit();
            }
        }
    }
}
