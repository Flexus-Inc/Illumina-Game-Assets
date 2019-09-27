using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caller : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    public void SceneManager_GoToScene(int index) {
        ScenesManager.GoToScene(index);
    }

    public void MusicManager_ChangeVolume(float intensity) {
        MusicManager.ChangeVolume(intensity);
    }

    public void MusicManager_ChangeMusic(int index) {
        MusicManager.ChangeMusic(index);
    }

    public void SoundManager_PlaySoundFX(int index) {
        SoundManager.PlaySoundFX(index);
    }

    public void SoundManager_ChangeVolume(float intensity) {
        SoundManager.ChangeVolume(intensity);
    }
}