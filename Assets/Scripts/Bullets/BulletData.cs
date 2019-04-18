using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEditor;

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
    public RuntimeAnimatorController animatorController;
    public Color color = Color.white;
    public Vector2 scale = new Vector2(1f, 1f);
    public Material material;

    [Header("Sound FX")]
    public string shootSample = "shoot1";
    [Range(0f, 1f)]
    public float shootSampleVolume = 1.0f;
    public string hitSample;
    [Range(0f, 1f)]
    public float hitSampleVolume = 1.0f;
    // TODO: maybe add flight sample?
    //public string flightSample;

    [Header("Collision")]
    public BulletShape collisionShape;
    public Vector2 collisionScale = new Vector2(1f, 1f);

    [Header("Behaviour")]
    public bool useSimpleMovement;
    [ConditionalHide("useSimpleMovement", false)]
    public string movementBehaviour;
    [ConditionalHide("useSimpleMovement", false)]
    public Vector2 movementScale = new Vector2(1f, 1f);
    [ConditionalHide("useSimpleMovement", false)]
    public float timeScale = 1f;
    [ConditionalHide("useSimpleMovement", false, true)]
    public string movementScript;
}
