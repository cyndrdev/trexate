using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeart : MonoBehaviour
{
    private int _health;
    private float _shieldDuration;
    private bool _isDamaging = false;
    private bool _isDying = false;

    public bool IsShielded
    {
        get => _shieldDuration > 0f;
    }

    public bool IsDamaging
    {
        get => _isDamaging;
    }

    public int Health
    {
        get => _health;
    }

    public int MaxHealth
    {
        get => GameConstants.PlayerMaxHealth;
    }

    private void Awake()
    {
        _health = MaxHealth;
    }

    public void Hit()
    {
        _health--;

        // u lose damaging status on hit
        _isDamaging = false;
    }

    public void Shield()
    {
        _shieldDuration = GameConstants.ShieldDuration;
    }

    public void SetDamaging()
    {
        _isDamaging = true;
    }

    void Update()
    {
        if (!_isDying && _health <= 0)
            Die();

        _shieldDuration -= Time.deltaTime;
    }

    private void Die()
    {
        _isDying = true;
        Game.GetPersistentComponent<DeathScreen>().StartDeathFade();
    }
}
