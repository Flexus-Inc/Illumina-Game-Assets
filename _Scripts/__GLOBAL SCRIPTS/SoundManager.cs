using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioClip[] SoundFXSources;
    public AudioClip DefaultSoundFX;
    public static AudioSource source;
    // Start is called before the first frame update
    void Start() {
        source = GetComponent<AudioSource>();
        source.clip = DefaultSoundFX;
        source.loop = false;
        source.playOnAwake = false;
    }

    // Update is called once per frame
    public void PlaySound(int soundIndex) {

        if (soundIndex < SoundFXSources.Length) {
            source.clip = SoundFXSources[soundIndex];
        }
        source.Play();
    }

    public static void PlaySoundFX(int soundIndex) {
        GameObject.Find("__SoundManager").GetComponent<SoundManager>().PlaySound(soundIndex);
    }

    public static void ChangeVolume(float intensity, float delay = 0) {
        GameObject.Find("__SoundManager").GetComponent<SoundManager>().ChangeSoundFXVolume(intensity, delay);
    }

    public void ChangeSoundFXVolume(float intensity, float delay = 0) {
        StartCoroutine(ChangeSoundVolume(intensity, delay));
    }
    IEnumerator ChangeSoundVolume(float intensity, float delay) {
        yield return new WaitForSeconds(delay);
        source.volume = intensity;
    }

}