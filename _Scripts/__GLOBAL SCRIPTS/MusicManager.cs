using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    //TODO: set default volume and looping if not set.
    public AudioClip[] MusicSources;
    public float[] MusicSourcesDefaultVolume;
    public bool[] MusicSourcesLoop;
    public int SplashMusicIndex = 0;
    AudioSource source;

    void Start() {
        source = GetComponent<AudioSource>();
    }
    public void Music(int index, bool hasTransition = true, float volume = 1, bool looping = false) {
        StartCoroutine(SwitchMusic(index, hasTransition, volume, looping));
    }

    public void ChangeMusic(int index) {
        Music(index);
    }
    public void ChangeMusicVolume(int intensity, bool transitioned = false) {
        if (transitioned) {
            StartCoroutine(TransitionVolume(intensity));
        } else {
            source.volume = intensity;
        }
    }

    public static void ChangeVolume(int intensity, bool transitioned = false) {
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

    public static void ChangeMusic(int index, bool hasTransition = false, float volume = 1, bool looping = false) {
        GameObject.Find("__MusicManager").GetComponent<MusicManager>().Music(index, hasTransition, volume, looping);
    }

    IEnumerator SwitchMusic(int index, bool hasTransition = false, float volume = 1, bool looping = false) {
        if (hasTransition) {
            while (source.volume > 0) {
                source.volume -= 0.05f;
                yield return new WaitForSeconds(0.025f);
            }
            source.clip = MusicSources[index];
            source.loop = looping;
            source.Play();
            while (source.volume < volume) {
                source.volume += 0.05f;
                yield return new WaitForSeconds(0.025f);
            }

        }
    }

    IEnumerator TransitionVolume(int intensity, bool stop = false) {

        if (source.volume > intensity) {
            while (source.volume > intensity) {
                source.volume -= 0.05f;
                yield return new WaitForSeconds(0.025f);
            }

        }

        if (source.volume < intensity) {
            while (source.volume < intensity) {
                source.volume += 0.05f;
                yield return new WaitForSeconds(0.025f);
            }
        }

    }

}