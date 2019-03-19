using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletShape
{
    Circle,
    Square,
    Rectangle
}

[CreateAssetMenu(fileName = "Data", menuName = "Bullet", order = 1)]
public class BulletData : ScriptableObject
{
    public Sprite sprite;
    public Vector2 scale;

    public float lifetime;

    public BulletShape collisionShape;
    public Vector2 collisionScale;

    public string movementBehaviour;

    public bool explodeOnInpact;
}
