using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseDisplay : MonoBehaviour
{
    private Text _display;

#pragma warning disable 0649
    [SerializeField]
    private bool _flash;
#pragma warning restore 0649

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
