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
    [Header("General")]
    public float lifetime;

    [Header("Visuals")]
    public Sprite sprite;
    public Vector2 scale;

    [Header("Collision")]
    public BulletShape collisionShape;
    public Vector2 collisionScale;

    [Header("Behaviour")]
    public string movementBehaviour;
    public Vector2 movementScale;
    public float timeScale;
    public bool explodeOnInpact;
}
