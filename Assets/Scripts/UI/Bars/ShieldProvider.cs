using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProvider : MonoBehaviour, IScaleProvider
{
    private PlayerHeart _heart;

    private void Start()
    {
        _heart = GameObject
            .FindGameObjectWithTag(GameConstants.PlayerController)
            .GetComponent<PlayerHeart>();
    }

    public float GetValue()
        => (_heart.IsShielded) ? _heart.ShieldLeft : 0f;
    public float GetSubValue()
        => (_heart.IsShielded) ? 1f : _heart.ShieldBuildup;
}
