using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeart : MonoBehaviour
{
    [SerializeField]
    private int _maxHitPoints;
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
