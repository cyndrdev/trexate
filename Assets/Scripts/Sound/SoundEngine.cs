#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundEngine : MonoBehaviour {
    [SerializeField]
    float _sfxVolume = 1.0f;

    [SerializeField]
    float _musicVolume = 1.0f;

    [SerializeField]
    float _sfxPitchVariance = .4f;

    [SerializeField]
    float _musicFadeTime = 3f;

    [SerializeField]
    AudioClip[] _soundEffects;
    [SerializeField]
    AudioClip[] _musicTracks;

    GameObject[] _musicObjects;
    GameObject _currentMusic;

    public void Awake()
    {
        preloadMusic();
        updateVolumes();
    }

    public void Start()
    {
        if (!_musicTracks.Any())
            return;

        PlayMusic(_musicTracks.First().name);
    }

    private void preloadMusic()
    {
        foreach (AudioClip track in _musicTracks)
        {
            GameObject musicObj = new GameObject(track.name);
            musicObj.transform.parent = transform;
            musicObj.transform.localPosition = Vector3.zero;

            AudioSource musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.clip = track;
            musicSource.volume = 0f;
            musicSource.loop = true;
        }
    }

    private void updateVolumes()
    {
        if (_currentMusic)
        {
            _currentMusic.GetComponent<AudioSource>().volume = _musicVolume;
        }
    }

    private float getRandomPitch()
    {
        return Random.Range(
            (1f / (_sfxPitchVariance + 1f)),
            (1f * (_sfxPitchVariance + 1f))
            );
    }

    public IEnumerator PlayAndDelete(GameObject sfxObj)
    {
        AudioSource audioSource = sfxObj.GetComponent<AudioSource>();
        audioSource.Play();
        yield return new WaitForSecondsRealtime(audioSource.clip.length * _sfxPitchVariance);
        Destroy(sfxObj);
    }

    public void PlaySFX(string clipname, bool varyPitch=true)
    {
        AudioClip clip = _soundEffects
            .Where((AudioClip a) => a.name == clipname)
            .First();

        if (clip == null)
        {
            Debug.Log("[SoundEngine]: tried to play sound effect '" + clipname + "', but it doesn't exist.");
            return;
        }

        // the clip we're looking for exits! let's play it!
        GameObject audioObj = new GameObject("audio_" + clipname);
        audioObj.transform.parent = transform;
        audioObj.transform.localPosition = Vector3.zero;

        float pitchMultiplier = varyPitch ? getRandomPitch() : 1f;
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = _sfxVolume;
        audioSource.pitch = pitchMultiplier;

        StartCoroutine(PlayAndDelete(audioObj));
    }

    public IEnumerator FadeMusicIn(AudioSource track)
    {
        track.volume = 0f;
        track.Play();

        float timeMultiplier = Mathf.PI / (2 * _musicFadeTime);
        float fadePosition = 0f;

        while (fadePosition < _musicFadeTime)
        {
            fadePosition += Time.deltaTime;
            track.volume = (1f - Mathf.Cos(fadePosition * timeMultiplier)) * _musicVolume;
            yield return new WaitForEndOfFrame();
        }

        track.volume = _musicVolume;
    }

    public IEnumerator FadeMusicOut(AudioSource track)
    {
        float timeMultiplier = Mathf.PI / (2 * _musicFadeTime);
        float fadePosition = 0f;

        while (fadePosition < _musicFadeTime)
        {
            fadePosition += Time.deltaTime;
            track.volume = Mathf.Cos(fadePosition * timeMultiplier) * _musicVolume;
            yield return new WaitForEndOfFrame();
        }

        track.volume = 0f;
        yield return new WaitForEndOfFrame();
        track.Stop();
    }

    private IEnumerator _playMusic(string trackname, bool waitForFade)
    {
        bool didstop = StopMusic();
        if (didstop && waitForFade)
        {
            yield return new WaitForSecondsRealtime(_musicFadeTime);
        }

        _currentMusic = transform.Find(trackname).gameObject;

        if (_currentMusic != null)
        {
            StartCoroutine(
                FadeMusicIn(
                    _currentMusic.GetComponent<AudioSource>()));
        }
        else
        {
            Debug.Log("[SoundEngine]: tried to play music track '" + trackname + "', but it doesn't exist!");
        }
    }

    public void PlayMusic(string trackname, bool waitForFade = true)
    {
        StartCoroutine(_playMusic(trackname, waitForFade));
    }

    public void PlayMusicNofade(string trackname)
    {
        StopMusicNofade();

        _currentMusic = transform.Find(trackname).gameObject;

        if (_currentMusic != null)
        {
            AudioSource source = _currentMusic.GetComponent<AudioSource>();
            source.volume = _musicVolume;
            source.Play();
        }
        else
        {
            Debug.Log("[SoundEngine]: tried to play music track '" + trackname + "', but it doesn't exist!");
        }
    }

    public bool StopMusic()
    {
        bool stopped = _currentMusic != null;

        if (stopped)
        {
            StartCoroutine(
                FadeMusicOut(
                    _currentMusic.GetComponent<AudioSource>()));
        }

        _currentMusic = null;
        return stopped;
    }

    public bool StopMusicNofade()
    {
        bool stopped = _currentMusic != null;

        if (stopped)
        {
            AudioSource source = _currentMusic.GetComponent<AudioSource>();
            source.volume = 0f;
            source.Stop();
        }

        return stopped;
    }
}
