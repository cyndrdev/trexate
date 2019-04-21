using System.Collections;
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
    public TimeTravelManager TimeTravelManager { get; private set; }
    public EnemyFactory EnemyFactory { get; private set; }

    public static T GetPersistentComponent<T>() where T : Component
    {
        T t = Instance.Persistant.GetComponent<T>();

        if (t == null)
            throw new System.Exception();

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
