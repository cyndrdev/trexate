using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelData[] _levels;
    private LevelData _currentLevel;

    void Start()
    {
        SetLevel(0);
    }

    public void SetLevel(int id)
    {
        _currentLevel = _levels[id];
    }
}
