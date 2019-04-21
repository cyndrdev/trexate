using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

[System.Serializable]
public struct WaveEntry
{
    public EnemyData Enemy;
    [Range(0f, 10f)]
    public float Delay;
    [Range(-1f, 1f)]
    public float xPosition;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Trexate/Wave", order = 3)]
public class WaveData : ScriptableObject
{
    // === assignable data === */
    public float Delay;
    public WaveEntry[] WaveEntries;

    /* === rest of the stuffs === */
    private float _length = -1f;
    private float _entryPosition;
    private int _wavePosition;
    public float Length
    {
        get
        {
            // return cache if existing
            if (_length > 0f)
                return _length;

            _length = Delay;
            foreach (var entry in WaveEntries)
            {
                _length += entry.Delay;
            }

            return _length;
        }
    }

    public WaveEntry? CurrentEntry
    {
        get => WaveEntries.GetNullable(_wavePosition);
    }

    public void Start()
    {
        _wavePosition = 0;
        _entryPosition = 0f;
    }

    public void Update()
    {
        _entryPosition += Time.deltaTime;
        while (CurrentEntry?.Delay < _entryPosition)
        {
            WaveEntry entry = CurrentEntry.GetValueOrDefault();
            _entryPosition -= entry.Delay;
            _wavePosition++;
            Game.GetPersistentComponent<EnemyFactory>().Spawn(entry.Enemy, entry.xPosition);
        }
    }
}
