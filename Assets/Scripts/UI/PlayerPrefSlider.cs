using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefSlider : MonoBehaviour
{
    public string _playerPref;

    public Slider _slider;

    void OnEnable()
    {
        if (PlayerPrefs.HasKey(_playerPref))
            _slider.value = PlayerPrefs.GetFloat(_playerPref);
    }

    public void SetLevel(float level)
    {
        PlayerPrefs.SetFloat(_playerPref, level);

        GameObject.FindGameObjectWithTag("GameController")
            .GetComponent<SoundEngine>()
            .UpdateVolumes();
    }
}
