using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class EnemyHeart : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private int _maxHitPoints;
#pragma warning restore 0649

    [SerializeField]
    [ReadOnly]
    private int _hitPoints;

    private IEnumerator hitFlashInstance;

    private static SoundEngine _soundEngine;
    private Material _material;

    void Start()
    {
        _hitPoints = _maxHitPoints;
        _soundEngine = Game.GetPersistentComponent<SoundEngine>();
        _material = gameObject.GetComponentInChildren<SpriteRenderer>().material;
    }
    
    public void DoDamage(int damage)
    {
        _hitPoints -= damage;
        _soundEngine.PlaySFX("hurt");
        GameConstants.GruntHitScore.AddToScore();
        // Debug.Log("[EnemyHeart]: took " + damage.ToString() + " damage!");

        if (_hitPoints < 0)
        {
            // die
            die();
        }
        else
        {
            if (hitFlashInstance != null)
                StopCoroutine(hitFlashInstance);

            hitFlashInstance = DoFlash();
            StartCoroutine(hitFlashInstance);
        }
    }

    private void die()
    {
        _soundEngine.PlaySFX("explosion");
        GameConstants.GruntKillScore.AddToScore();
        Destroy(gameObject);
    }

    private IEnumerator DoFlash()
    {
        int flashFrames = (int)(GameConstants.HitFlashFrames * Time.timeScale);
        int flashFadeFrames = (int)(GameConstants.HitFlashFade * Time.timeScale);

        _material.SetFloat("_HitAmount", GameConstants.HitFlashPeak);

        for (int i = 0; i < flashFrames; i++)
            yield return new WaitForEndOfFrame();

        for (int i = 0; i < flashFadeFrames; i++)
        {
            _material.SetFloat("_HitAmount", GameConstants.HitFlashPeak * (flashFadeFrames - i) / (flashFadeFrames + 1f));
            yield return new WaitForEndOfFrame();
        }

        _material.SetFloat("_HitAmount", 0f);
    }
}
