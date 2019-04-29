using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Heal,
    Damage,
    Shield
}

public class Pickup : MonoBehaviour
{
    float _minY;
    public PickupType _type;

    void Start()
    {
        _minY = Game.Instance.PixelPerfectCamera.OrthoScale + GameConstants.PickupDespawnMargin;
    }

    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * GameConstants.PickupFallRate);

        if (Mathf.Abs(transform.position.y) >= _minY)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var heart = coll.gameObject.GetComponent<PlayerHeart>();
        if (heart == null)
            return;

        if (HandleSpecificActions(heart))
            Destroy(this.gameObject);
    }

    bool HandleSpecificActions(PlayerHeart heart)
    {
        switch (_type)
        {
            case PickupType.Heal:
                if (heart.IsFullHealth)
                    return false;

                heart.Heal();
                return true;

            case PickupType.Damage:
                if (heart.IsDamaging)
                    return false;

                heart.AddDamaging();
                return true;

            case PickupType.Shield:
                if (heart.IsShielded)
                    return false;

                heart.AddShield();
                return true;

            default:
                return false;
        }
    }
}
