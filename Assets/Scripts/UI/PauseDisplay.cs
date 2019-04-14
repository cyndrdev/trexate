using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseDisplay : MonoBehaviour
{
    private Text _display;

    [SerializeField]
    private bool _flash;

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
        bool flashState;

        flashState = (
           !_flash || // if we enable flashing, flashState will always be true
           Mathf.RoundToInt(
               Time.unscaledTime / GameConstants.PauseDisplayFlashRate
           ) % 2 == 0
        );

        _display.enabled = (
            Time.timeScale == 0 && flashState
        );
    }
}
