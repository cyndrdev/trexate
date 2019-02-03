using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public GameObject Persistant;
    public Transform BulletRoot;

    public SoundEngine SoundEngine { get; private set; }
    public InputManager InputManager { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("[Game]: Main game instance already exists, aborting initialisation.");
            Destroy(this);
        }

        Instance = this;
        SoundEngine = Persistant.GetComponent<SoundEngine>();
        InputManager = Persistant.GetComponent<InputManager>();

        DontDestroyOnLoad(this.gameObject);
    }
}
