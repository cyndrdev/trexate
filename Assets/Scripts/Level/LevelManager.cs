using System.Collections;
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

    private float _startDate;
    private float _endDate;
    private float _dateJump;

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
            // debug: loads credits after 1st world
            //LoadCredits(); return;
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
        Game.GetPersistentComponent<Credits>().Begin();
    }

    void Update()
    {
        if (CurrentLevel == null)
            return;

        CurrentLevel.Update();

        if (CurrentLevel.IsJumping) return;

        if (CurrentLevel.CurrentWave == null)
        {
            // level is over, on to the next
            NextLevel();
        }

    }

    IEnumerator ChangeLevel()
    {
        bool hasFadeOut = (_currentLevelId != 0);
        float jumpDuration = GameConstants.TimeJumpDuration / (hasFadeOut ? 1f : 2f);

        // start music fade
        var engine = Game.GetPersistentComponent<SoundEngine>();
        //if (hasFadeOut)
            Game.GetPersistentComponent<SoundEngine>().PlayMusic(CurrentLevel.Music);
        //else
            //Game.GetPersistentComponent<SoundEngine>().PlayMusicNofade(CurrentLevel.Music);

        // jump our time
        _startDate = _timeTravelManager.GetT(CurrentLevel.StartYear);
        _endDate = _timeTravelManager.GetT(CurrentLevel.EndYear);
        _timeTravelManager.DoJump(_startDate, jumpDuration);

        // first, fade the level out
        if (hasFadeOut)
        {

            _fader.FadeTo(1f, GameConstants.AreaSwitchFadeDuration);
            yield return new WaitForSeconds(
                GameConstants.AreaSwitchFadeDuration
            );

            // load the new background
            _background.SwapLayer(_currentLevelId);

            Game.GetPersistentComponent<BulletFactory>().ClearBullets();
        }

        // load new date target
        _timeTravelManager.Target = _timeTravelManager.GetT(CurrentLevel.EndYear);

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
