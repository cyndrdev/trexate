using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyExit : MonoBehaviour
{
    private float _time = 0;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // first frame the key is pressed, reset timer
                _time = 0;
            }

            _time += Time.deltaTime;

            if (_time > GameConstants.EmergencyExitDuration)
            {
                Application.Quit();
            }
        }
    }
}
