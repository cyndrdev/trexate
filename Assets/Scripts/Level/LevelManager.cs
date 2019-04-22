﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelData[] _levels;
    private int _currentLevelId;

    private TimeTravelManager _timeTravelManager;

    [Header("UI References")]

    [SerializeField]
    private GameObject _fadeObject;
    private ScreenFader _fader;

    [SerializeField]
    private GameObject _titleObject;
    private NewAreaTitle _newAreaTitle;

    [SerializeField]
    private GameObject _bgObject;
    private ParallaxBackground _background;

    public bool IsJumping { get => (CurrentLevel == null) ? false : CurrentLevel.IsJumping; }

    protected LevelData CurrentLevel
    {
        get => _levels.GetAtIndex(_currentLevelId);
    }

    void Start()
    {
        _timeTravelManager = Game.GetPersistentComponent<TimeTravelManager>();
        _fader = _fadeObject.GetComponent<ScreenFader>();
        _background = _bgObject.GetComponent<ParallaxBackground>();
        _newAreaTitle = _titleObject.GetComponent<NewAreaTitle>();
        Debug.Log("there are " + _levels.Length + " levels.");
        SetLevel(0);
    }

    public void NextLevel()
    {
        if (_currentLevelId < _levels.Length)
        {
            if (_currentLevelId == _levels.Length - 1)
                LoadCredits();
            else
                SetLevel(_currentLevelId + 1);
        }
    }

    public void SetLevel(int id)
    {
        Debug.Log("Changing level to " + id);
        _currentLevelId = id;

        if (CurrentLevel == null)
            throw new System.Exception();

        CurrentLevel.Start();
        StartCoroutine(ChangeLevel());
    }

    public void LoadCredits()
    {
        _currentLevelId++;
        Debug.Log("credits time uwu");
    }

    void Update()
    {
        if (CurrentLevel == null)
            return;

        CurrentLevel.Update();

        if (CurrentLevel.IsJumping)

        if (CurrentLevel.CurrentWave == null)
        {
            // level is over, on to the next
            NextLevel();
        }

    }

    IEnumerator ChangeLevel()
    {
        // first, fade the level out
        if (_currentLevelId != 0)
        {
            _fader.FadeTo(1f, GameConstants.AreaSwitchFadeDuration);
            yield return new WaitForSeconds(
                GameConstants.AreaSwitchFadeDuration
            );

            // load the new background
            _background.SwapLayer(_currentLevelId);

            // TODO load dates?
        }

        // and fade back in
        _fader.FadeTo(0f, GameConstants.AreaSwitchFadeDuration);
        yield return new WaitForSeconds(
            GameConstants.AreaSwitchFadeDuration
        );

        // now do the title fade
        yield return new WaitForSeconds(
            _newAreaTitle.DisplayText(CurrentLevel.levelName)
        );
    }
}
