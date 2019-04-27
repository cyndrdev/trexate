using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public GameObject Persistant;
    public Transform BulletRoot;
    public Transform EnemyRoot;

    private GameObject _player;

    private bool _isPaused;

    public bool IsPaused
    {
        get => _isPaused;
    }

    private Dictionary<Type, Component> _instances
        = new Dictionary<Type, Component>();

    public Vector3 PlayerPosition {
        get
        {
            if (_player == null)
                UpdatePlayerReference();

            return _player.transform.position;
        }
        set
        {
            if (_player == null)
                UpdatePlayerReference();

            _player.transform.position = value;
        }
    }

    private void UpdatePlayerReference()
    {
        _player = GameObject.FindGameObjectWithTag(GameConstants.PlayerController);
    }

    public PixelPerfectCamera PixelPerfectCamera { get; private set; }

    public static T GetPersistentComponent<T>() where T : Component
    {
        // check our cache
        if (Instance._instances.ContainsKey(typeof(T)))
            return (T)Instance._instances[typeof(T)];

        // try a getcomponent instead
        T t = Instance.Persistant.GetComponent<T>();
        if (t == null) 
            throw new System.Exception();

        // add to the cache
        Instance._instances.Add(typeof(T), t);
        return t;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("[Game]: Main game instance already exists, aborting initialisation.");
            Destroy(Game.Instance.gameObject);
        }

        Time.timeScale = 1f;

        Instance = this;

        PixelPerfectCamera  = Camera.main.GetComponent<PixelPerfectCamera>();
        if (PixelPerfectCamera  == null)    throw new System.Exception();

        UpdatePlayerReference();

        // DontDestroyOnLoad(this.gameObject);
    }

    public void TogglePause()
    {
        if (_isPaused && Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            _isPaused ^= true;
        }
        else if (!_isPaused && Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
            _isPaused ^= true;
        }
    }
}
