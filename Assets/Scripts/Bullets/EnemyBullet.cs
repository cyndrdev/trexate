using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    protected private void Update()
    {
        var verticalMovement = Vector3.right * Time.deltaTime;
        var horizontalMovement = Vector3.up * Time.deltaTime;
        transform.Translate(verticalMovement + horizontalMovement);
        print("henlo from enemybullet");
    }

    protected virtual void OnHit()
    {
        Destroy(this.gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerHeart player = collider.GetComponent<PlayerHeart>();
        if (player == null) return;

        player.Hit();
        this.OnHit();
    }
}
