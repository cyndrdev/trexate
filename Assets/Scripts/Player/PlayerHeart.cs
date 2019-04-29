using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeart : MonoBehaviour
{
    private int _health;

    private float _shieldDuration;
    private int _shieldBuildup = 0;

    private bool _isDamaging = false;
    private int _damageBuildup = 0;

    private bool _isDying = false;
    private SoundEngine _soundEngine;

    public bool IsShielded
        => _shieldDuration > 0f;

    public float ShieldLeft
        => _shieldDuration;

    public float ShieldBuildup
        => (float)_shieldBuildup / GameConstants.AbilityBuildLimit;

    public bool IsDamaging
        => _isDamaging;

    public float DamageBuildup
        => (float)_damageBuildup / GameConstants.AbilityBuildLimit;

    public int Health
        => _health;

    public bool IsFullHealth
        => _health == MaxHealth;

    public int MaxHealth
        => GameConstants.PlayerMaxHealth;

    private void Awake()
    {
        _health = MaxHealth;
    }

    private void Start()
    {
        _soundEngine = Game.GetPersistentComponent<SoundEngine>();
    }

    public void Heal()
    {
        _health++;

        if (_health > MaxHealth)
            _health = MaxHealth;
    }

    public void Hit()
    {
        if (IsShielded)
        {
            _soundEngine.PlaySFX("hit_miss");
            return;
        }
        else
        {
            _health--;

            // u lose damaging status on hit
            _isDamaging = false;
        }
    }

    public void AddShield()
    {
        _shieldBuildup++;
        if (_shieldBuildup >= GameConstants.AbilityBuildLimit)
        {
            _shieldBuildup = 0;
            _shieldDuration = GameConstants.ShieldDuration;
        }
    }

    public void AddDamaging()
    {
        _damageBuildup++;
        if (_damageBuildup >= GameConstants.AbilityBuildLimit)
        {
            _damageBuildup = 0;
            _isDamaging = true;
        }
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
