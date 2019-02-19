using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    protected virtual void OnHit()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerHeart player = collider.GetComponent<PlayerHeart>();
        if (player == null) return;

        player.Hit();
        this.OnHit();
    }
}
