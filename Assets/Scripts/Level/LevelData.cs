using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Extensions;

[CreateAssetMenu(fileName = "LevelData", menuName = "Trexate/Level", order = 2)]
public class LevelData : ScriptableObject
{
    /* === assignable data === */
    public string levelName;

    [Header("Visuals")]
    public int StartYear;
    public int EndYear;
    public string Background;

    [Header("Waves")]
    public WaveData[] Waves;

    [Header("Sound")]
    public string Music;

    /* === methods & fields === */

    public bool IsJumping { get => _jumpTimer > 0f; }

    private float _startT;
    private float _endT;

    private float _length = -1f;
    private int _currentWaveId = 0;
    private float _jumpTimer;

    public float Length
    {
        get => Waves.Sum(wave => wave.Length);
    }

    public WaveData CurrentWave
    {
        get => Waves.GetAtIndex(_currentWaveId);
    }

    public void Start()
    {
        Waves[0].Start();
        _currentWaveId = 0;
        _jumpTimer = GameConstants.TimeJumpDuration;

        _startT = Game.GetPersistentComponent<TimeTravelManager>().GetT(StartYear);
        _endT = Game.GetPersistentComponent<TimeTravelManager>().GetT(EndYear);
    }

    public void Update()
    {
        if (_jumpTimer > 0f)
        {
            _jumpTimer -= Time.deltaTime;
        }
        else
        {
            if (CurrentWave == null)
                return;

            CurrentWave.Update();
            if (
                CurrentWave.CurrentEntry == null &&
                !Game.GetPersistentComponent<EnemyFactory>().HasActiveEnemies
            )
            {
                // wave is over, start a jump!
                Debug.Log("starting jump...");
                _jumpTimer = GameConstants.TimeJumpDuration;
                _currentWaveId++;

                if (CurrentWave != null)
                {
                    // start our time jump
                    float t = _startT + ((_endT - _startT) / ((float)_currentWaveId + 1) / Waves.Length);

                    Game.GetPersistentComponent<TimeTravelManager>()
                        .DoJump(t, GameConstants.TimeJumpDuration);

                    CurrentWave.Start();
                }
            }
        }
    }
}
