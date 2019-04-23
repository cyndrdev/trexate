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

    private Dictionary<Type, Component> _instances
        = new Dictionary<Type, Component>();

    public Vector3 PlayerPosition {
        get => _player.transform.position;
        set => _player.transform.position = value;
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
            Debug.LogError("[Game]: Main game instance already exists, aborting initialisation.");
            Destroy(this);
        }

        Instance = this;

        PixelPerfectCamera  = Camera.main.GetComponent<PixelPerfectCamera>();
        if (PixelPerfectCamera  == null)    throw new System.Exception();

        _player = GameObject.FindGameObjectWithTag(GameConstants.PlayerController);

        DontDestroyOnLoad(this.gameObject);
    }

    public void TogglePause()
        => Time.timeScale = (Time.timeScale == 0) ? 1f : 0f;
}
