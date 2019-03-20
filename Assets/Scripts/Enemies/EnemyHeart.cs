using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeart : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private int _maxHitPoints;
#pragma warning restore 0649

    private int _hitPoints;

    private static SoundEngine _soundEngine;

    void Start()
    {
        _hitPoints = _maxHitPoints;
        _soundEngine = Game.Instance.SoundEngine;
    }
    
    public void DoDamage(int damage)
    {
        _hitPoints -= damage;
        _soundEngine.PlaySFX("hurt");
        Debug.Log("[EnemyHeart]: took " + damage.ToString() + " damage!");

        if (_hitPoints < 0)
        {
            // die
            die();
        }
    }

    private void die()
    {
        _soundEngine.PlaySFX("explosion");
        Destroy(gameObject);
    }
}
