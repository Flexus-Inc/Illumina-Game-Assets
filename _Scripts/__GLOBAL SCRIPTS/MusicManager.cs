using System;
using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    //TODO: set default volume and looping if not set.
    public AudioClip[] MusicSources;
    public float[] MusicSourcesDefaultVolume;
    public bool[] MusicSourcesIsLooping;
    public AudioClip DefaultMusic;
    public int SplashMusicIndex = 0;
    public AudioSource source;

    void Awake() {
        source = GetComponent<AudioSource>();
        source.loop = false;
        source.playOnAwake = false;
        if (MusicSources.Length != MusicSourcesDefaultVolume.Length) {
            throw new ArgumentException("Length", "MusicSourcesDefaultVolume.Length doesn't match the length of MusicSources array");
        }
        if (MusicSources.Length != MusicSourcesIsLooping.Length) {
            throw new ArgumentException("Length", "MusicSourcesIsLooping.Length doesn't match the length of MusicSources array");
        }
    }

    void Start() {
        SettingsController.UpdateVolume();
        Debug.Log("Volume : " + SettingsController.GetMusicVolume());
    }

    public void Music(int index, bool hasTransition = true, float volume = 2, int looping = -1) {
        var islooping = false;

        if (looping < 0) {
            if (index < MusicSources.Length) {
                islooping = MusicSourcesIsLooping[index];
            }
        } else if (looping == 1) {
            islooping = true;
        }
        if (index < MusicSources.Length) {
            volume = volume > 1 ? MusicSourcesDefaultVolume[index] : volume;
        }
        StartCoroutine(SwitchMusic(index, hasTransition, volume, islooping));
    }

    public void ChangeMusic(int index) {
        Music(index);
    }
    public void ChangeMusicVolume(float intensity, bool transitioned = false) {
        if (transitioned) {
            StartCoroutine(TransitionVolume(intensity));
        } else {
            source.volume = intensity * SettingsController.GetMusicVolume();
        }
    }

    public static void ChangeVolume(float intensity, bool transitioned = false) {
        GameObject.Find("__MusicManager").GetComponent<MusicManager>().ChangeMusicVolume(intensity, true);
    }
    public void StopMusic(bool transitioned = true) {
        if (transitioned) {
            StartCoroutine(TransitionVolume(0, true));
        } else {
            source.volume = 0;
        }
    }
    public static void StopMusic() {
        GameObject.Find("__MusicManager").GetComponent<MusicManager>().StopMusic(true);
    }

    public static void ChangeMusic(int index, bool hasTransition = false, float volume = 1, int looping = -1) {
        GameObject.Find("__MusicManager").GetComponent<MusicManager>().Music(index, hasTransition, volume, looping);
    }

    IEnumerator SwitchMusic(int index, bool hasTransition = false, float volume = 1, bool looping = false) {
        var dist = source.volume / 20;
        volume *= SettingsController.GetMusicVolume();
        if (hasTransition) {
            while (source.volume > 0) {
                source.volume -= dist;
                yield return new WaitForSeconds(0.025f);
            }
        }
        source.clip = DefaultMusic;
        if (index < MusicSources.Length) {
            source.clip = MusicSources[index];
        }
        source.loop = looping;
        source.Play();
        dist = volume / 20;
        if (hasTransition) {
            while (source.volume < volume) {
                source.volume += dist;
                yield return new WaitForSeconds(0.025f);
            }
        }
    }

    IEnumerator TransitionVolume(float intensity, bool stop = false) {

        intensity *= SettingsController.GetMusicVolume();
        if (source.volume > intensity) {
            var dist = (source.volume - intensity) / 20;
            while (source.volume > intensity) {
                source.volume -= dist;
                yield return new WaitForSeconds(0.025f);
            }

        }

        if (source.volume < intensity) {
            var dist = (intensity - source.volume) / 20;
            while (source.volume < intensity) {
                source.volume += dist;
                yield return new WaitForSeconds(0.025f);
            }
        }

    }

}