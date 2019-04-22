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
    [SerializeField]
    private Text _anorLondo;

    public bool IsJumping { get => (CurrentLevel == null) ? false : CurrentLevel.IsJumping; }

    protected LevelData CurrentLevel
    {
        get => _levels.GetAtIndex(_currentLevelId);
    }

    void Start()
    {
        _timeTravelManager = Game.GetPersistentComponent<TimeTravelManager>();
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
        // do the dark souls text thing
        string name = CurrentLevel.levelName;
        _anorLondo.text = name;
        yield return new WaitForSeconds(2f);
        _anorLondo.text = "";
    }
}
