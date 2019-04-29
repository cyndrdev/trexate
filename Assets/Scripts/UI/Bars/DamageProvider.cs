using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProvider : MonoBehaviour, IScaleProvider
{
    private PlayerHeart _heart;

    private void Start()
    {
        _heart = GameObject
            .FindGameObjectWithTag(GameConstants.PlayerController)
            .GetComponent<PlayerHeart>();
    }

    public float GetValue()
        => _heart.IsDamaging ? 1f : 0f;
    public float GetSubValue()
        => _heart.IsDamaging ? 1f : _heart.DamageBuildup;
}
