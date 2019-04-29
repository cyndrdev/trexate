using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string _menuMusic;
    public GameObject _fadeObject;
    private ScreenFader _screenFader;

    void Start()
        => StartCoroutine(Bootstrap());

    public void StartGame()
        => StartCoroutine(StartStage());

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private IEnumerator Bootstrap()
    {
        _screenFader = _fadeObject.GetComponent<ScreenFader>();

        for (int i = 0; i < 4; i++)
            yield return new WaitForEndOfFrame();

        GameObject.FindGameObjectWithTag("GameController")
            .GetComponent<SoundEngine>()
            .PlayMusicNofade(_menuMusic);
    }

    private IEnumerator StartStage()
    {
        GameObject.FindGameObjectWithTag("GameController")
            .GetComponent<SoundEngine>()
            .StopMusic();

        _screenFader.FadeTo(1f, 1.0f);
        yield return new WaitForSecondsRealtime(1.5f);

        Debug.Log("Starting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
