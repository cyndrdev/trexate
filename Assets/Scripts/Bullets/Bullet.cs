using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int _frameCounter = 0;

    public void Update()
    {
        Debug.Log("im updatin");
        // this check is expensive, only run it every 10 frames
        if (_frameCounter++ % 10 == 0)
        {
            var viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            if (isOffScreen(viewportPos.x) || isOffScreen(viewportPos.y))
            {
                Destroy(this.gameObject);
            }
        }
    }

    private bool isOffScreen(float f)
    {
        float diff = 0.5f - Mathf.Abs(f - 0.5f);
        return diff < -GameConstants.BulletOffScreenMargin;
    }
}
