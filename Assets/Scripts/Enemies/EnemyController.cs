﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Extensions;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyData _data;
    private IEnemyState _currentState;
    private int _currentStateId;

    [Header("Debug State")]
    [SerializeField]
    [ReadOnly]
    private bool _isInvulnerable;
    private float _invulnTime;

    [SerializeField]
    [ReadOnly]
    private int _hitPoints;

    private IEnumerator _hitFlashInstance;
    private static SoundEngine _soundEngine;
    private SpriteRenderer _renderer;
    private Material _material;

    public bool IsInvulnerable
    {
        get => _isInvulnerable;
        set {
            if (value)
                _invulnTime = 0f;
            else
                _invulnTime = -1f;

            _isInvulnerable = value;
        }
    }

    public bool IsVisible
    {
        get => _renderer.enabled;
        set => _renderer.enabled = value;
    }

    private int NextTrigger
    {
        get {
            if (_data.Stages.Count <= _currentStateId + 1)
                return -1;

            return _data.Stages[_currentStateId + 1].HealthTrigger;
        }
    }

    public void Initialise(EnemyData data)
    {
        _data = data;

        /* === sort our transform out === */
        gameObject.layer = GameConstants.EnemyLayer;

        /* === add a rigidbody for collisions to work === */
        var rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = 0.00001f;

        /* === create our collider === */
        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.offset = Vector2.zero;
        collider.size = _data.collisionScale;
        collider.edgeRadius = 0.2f;

        /* === initialise our graphics object === */
        if (transform.childCount == 0)
        {
            GameObject graphics = new GameObject("Graphics");
            graphics.transform.parent = transform;
            graphics.transform.localPosition = Vector2.zero;

            _renderer = graphics.AddComponent<SpriteRenderer>();
            _renderer.sprite = _data.sprite;
            _renderer.sortingLayerName = GameConstants.EntitySortLayer;
            _renderer.flipX = _data.flipX;
            _renderer.flipY = _data.flipY;

            Material material = new Material(Shader.Find(GameConstants.EnemyShader));

            material.mainTexture = _renderer.sprite.texture;
            _renderer.material = material;

            _renderer.transform.localScale = _data.collisionScale;
        }

        // TODO: animation???
    }

    /* === unity methods === */
    private void Start()
    {
        _soundEngine = Game.GetPersistentComponent<SoundEngine>();
        _renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        _material = _renderer.material;
        _hitPoints = _data.MaxHealth;
        SetState(0);
    }

    private void Update()
    {
        UpdateColors();

        // TODO:
        // - remove `EnemyHeart` and manage health here
        // - update _currentState based on health triggers

        if (_currentState != null)
            _currentState.Update();
    }

    /* === damage handlers === */
    private void UpdateColors()
    {

        Color newColor;

        if (_isInvulnerable)
        {
            _invulnTime += Time.deltaTime;
            float amt = -Mathf.Cos(
                _invulnTime * 
                GameConstants.EnemyInvulnFlashRate * 
                2f * Mathf.PI
            );
            amt = (-amt).SignalToScale();
            newColor = Color.Lerp(
                Color.white,
                GameConstants.EnemyInvulnFlashColor,
                amt
            );
        }
        else
        {
            newColor = Color.white;
        }

        _renderer.color = newColor;
    }

    public int Health { get => _hitPoints; }

    public void Damage(int damage, bool overrideInvulnerability = false)
    {
        if (_isInvulnerable && !overrideInvulnerability)
            return;

        if (damage < 0)
        {
            throw new System.Exception("[EnemyController]: tried to deal negative damage!");
        }

        _hitPoints -= damage;

        // debug: enemies die in one hit
        //if (!overrideInvulnerability) Die(true);

        if (!overrideInvulnerability)
            DamageHooks();

        CheckTriggers();
    }

    public void DamageTo(int health, bool overrideInvulnerability = false)
    {
        if (_isInvulnerable && !overrideInvulnerability)
            return;

        if (_hitPoints < health)
        {
            throw new System.Exception("[EnemyController]: tried to call DamageTo to set a health value greater than current health.");
        }

        _hitPoints = health;

        if (!overrideInvulnerability)
            DamageHooks();

        CheckTriggers();
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
        {
            throw new System.Exception("[EnemyController]: tried to heal negative health!");
        }
        _hitPoints += amount;
        CheckTriggers();
    }

    public void Die(bool overrideSafety = false)
    {
        if (!overrideSafety)
        {
            if (_hitPoints > 0)
                return;

            if (_isInvulnerable)
                return;
        }

        // okay, we gotta die now ://
        // first, end our behaviour
        if (_currentState != null)
            _currentState.End();

        if (_data.deathSound != "")
            _soundEngine.PlaySFX(_data.deathSound);

        //FIXME: proper scores?
        _data.killScore.AddToScore();

        /*
        Game.GetPersistentComponent<PickupFactory>()
            .SpawnRandom(transform.position, _data.Pickups);
        */

        for (int i = 0; i < _data.SpawnAttempts; i++)
        {
            if (UnityEngine.Random.Range(0f, 1f) >= 1f - _data.SpawnChance)
            {
            Game.GetPersistentComponent<PickupFactory>()
                .SpawnRandom(transform.position, _data.Pickups);
            }
        }

        Destroy(gameObject);
    }

    public void DamageHooks()
    {
        // damage sfx
        if (_data.hitSound != "")
            _soundEngine.PlaySFX(_data.hitSound);

        // hitflash
        if (_hitFlashInstance != null)
            StopCoroutine(_hitFlashInstance);

        _hitFlashInstance = DoHitFlash();
        StartCoroutine(_hitFlashInstance);

        // points
        _data.hitScore.AddToScore();
    }

    /* === helper methods === */
    private void CheckTriggers()
    {
        //Debug.Log("checking triggers, next trigger: " + NextTrigger + ", current health: " + _hitPoints);
        // special death case
        if (_hitPoints <= 0)
        {
            if (NextTrigger == 0)
            {
                // we have a special death state,
                // let it handle our demise
                _hitPoints = 0;
                SetState(++_currentStateId);
            }
            else
            {
                Die(true);
            }
        }

        // are we at a trigger point?
        if (_hitPoints <= NextTrigger)
        {
            // _hitPoints = NextTrigger;
            SetState(++_currentStateId);
        }
    }

    private void SetState(int id)
    {
        // first, end our current state cleanly
        if (_currentState != null)
            _currentState.End();

        if (id >= _data.Stages.Count || id == -1) 
            return;

        string stateName = _data.Stages[id].StageScript;

        // to the unlucky soul reading this:
        // enemy behaviour is instantiated via reflection.
        // this is a terrible idea and you should never do it.
        // it was also the fastest way i could think of to get this working.
        // i hope you can forgive me.

        Type objType = Type.GetType(stateName);

        if (objType != null)
        {
            // Debug.Log("found type");
            if (objType.GetInterfaces().Contains(typeof(IEnemyState)))
            {
                // we successfully reflected the type we want, not bad!
                // now let's create an instance
                _currentState = (IEnemyState)Activator.CreateInstance(objType);

                // and start it!
                _currentState.Start(gameObject, this);

                return;
            }
            throw new System.Exception("[EnemyController]: tried to instantiate state with class '" + stateName + "', however that class doesn't implement IEnemyState.");
        }

        throw new System.Exception("[EnemyController]: tried to instantiate state with class name '" + stateName + "', however no such class exists.");
    }

    /* === coroutines === */
    private IEnumerator DoHitFlash()
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
