using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseDisplay : MonoBehaviour
{
    private Text _display;

    // Start is called before the first frame update
    void Start()
    {
        _display = this.GetComponent<Text>();

        if (_display == null)
            throw new System.Exception();
    }

    // Update is called once per frame
    void Update()
    {
        _display.enabled = (Time.timeScale == 0);
    }
}
