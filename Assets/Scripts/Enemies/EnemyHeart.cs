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
    private Material _material;

    void Start()
    {
        _hitPoints = _maxHitPoints;
        _soundEngine = Game.Instance.SoundEngine;
        _material = gameObject.GetComponentInChildren<SpriteRenderer>().material;
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
        else
        {
            StartCoroutine(DoFlash());
        }
    }

    private void die()
    {
        _soundEngine.PlaySFX("explosion");
        Destroy(gameObject);
    }

    private IEnumerator DoFlash()
    {
        int flashFrames = (int)(GameConstants.HitFlashFrames * Time.timeScale);
        int flashFadeFrames = (int)(GameConstants.HitFlashFade * Time.timeScale);

        _material.SetFloat("_HitAmount", 1f);

        for (int i = 0; i < flashFrames; i++)
            yield return new WaitForEndOfFrame();

        for (int i = 0; i < flashFadeFrames; i++)
        {
            _material.SetFloat("_HitAmount", (flashFadeFrames - i) / (flashFadeFrames + 1f));
            yield return new WaitForEndOfFrame();
        }

        _material.SetFloat("_HitAmount", 0f);
    }
}
