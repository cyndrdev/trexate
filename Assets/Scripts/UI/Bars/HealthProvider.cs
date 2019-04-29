using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HealthProvider : MonoBehaviour, IScaleProvider
{
    private PlayerHeart _heart;
    private List<float> _healthHistory;
    private float _subValueLerp;

    float CurrentHealthValue
    {
        get => (float)_heart.Health / _heart.MaxHealth;
    }

    void Start()
    {
        _heart = GameObject
            .FindGameObjectWithTag(GameConstants.PlayerController)
            .GetComponent<PlayerHeart>();

        float startValue = CurrentHealthValue;

        _healthHistory = new List<float>();
        for (int i = 0; i < GameConstants.HealthBarValues; i++)
            _healthHistory.Add(startValue);

        StartCoroutine(HealthStorage());
    }

    void Update()
    {
        _subValueLerp = Mathf.Lerp(
            _subValueLerp, 
            _healthHistory.First(), 
            GameConstants.HealthBarLerp);

        if (_healthHistory.Last() > _healthHistory.First())
        {
            // if we recently regained health, reset our history
            float value = _healthHistory.Last();
            for (int i = 0; i < GameConstants.HealthBarValues; i++)
                _healthHistory[i] = value;
        }
    }

    public float GetValue()
        => CurrentHealthValue;

    public float GetSubValue()
        => _subValueLerp;

    IEnumerator HealthStorage()
    {
        float delay = GameConstants.HealthBarDelay / GameConstants.HealthBarValues;
        while (true)
        {
            yield return new WaitForSecondsRealtime(delay);
            _healthHistory.RemoveAt(0);
            _healthHistory.Add(CurrentHealthValue);
        }
    }
}
