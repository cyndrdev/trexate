﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public GameObject Persistant;
    public Transform BulletRoot;

    private GameObject _player;
    public Vector3 PlayerPosition {
        get => _player.transform.position;
        set => _player.transform.position = value;
    }

    public SoundEngine SoundEngine { get; private set; }
    public InputManager InputManager { get; private set; }
    public BulletFactory BulletFactory { get; private set; }
    public PixelPerfectCamera PixelPerfectCamera { get; private set; }
    public GlobalState GlobalState { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("[Game]: Main game instance already exists, aborting initialisation.");
            Destroy(this);
        }

        Instance = this;

        SoundEngine         = Persistant.GetComponent<SoundEngine>();
        InputManager        = Persistant.GetComponent<InputManager>();
        BulletFactory       = Persistant.GetComponent<BulletFactory>();
        GlobalState         = Persistant.GetComponent<GlobalState>();

        PixelPerfectCamera  = Camera.main.GetComponent<PixelPerfectCamera>();

        if (SoundEngine         == null)    throw new System.Exception();
        if (InputManager        == null)    throw new System.Exception();
        if (BulletFactory       == null)    throw new System.Exception();
        if (PixelPerfectCamera  == null)    throw new System.Exception();
        if (GlobalState         == null)    throw new System.Exception();

        _player = GameObject.FindGameObjectWithTag(GameConstants.PlayerController);

        DontDestroyOnLoad(this.gameObject);
    }
}
