using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using Illumina.Models;
using UnityEngine;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour {
    public Slider MusicSlider;
    public Slider SoundFXSlider;
    public Slider MasterSlider;
    private int VolumeType;
    SettingsData Data;
    void Start() {
        Data = new SettingsData();
        Data = SettingsController.LoadSettingsData();
        UpdateSliderValues();
        UpdateVolume();
        MusicManager.ChangeMusic(2, true, Data.MasterVolume * Data.MusicVolume, 1);
    }

    public void UpdateSettings(int type) {
        switch (type) {
            case 1:
                Data.MusicVolume = MusicSlider.value;
                break;
            case 2:
                Data.SoundFXVolume = SoundFXSlider.value;
                break;
            default:
                Data.MasterVolume = MasterSlider.value;
                break;
        }
        UpdateVolume();
        SettingsController.SaveSettingsData(Data);
    }

    public void UpdateSliderValues(bool updateData = false) {
        Data = updateData ? SettingsController.LoadSettingsData() : Data;
        MusicSlider.normalizedValue = Data.MusicVolume;
        SoundFXSlider.normalizedValue = Data.SoundFXVolume;
        MasterSlider.normalizedValue = Data.MasterVolume;
    }

    public void UpdateVolume() {
        MusicManager.source.volume = Data.MusicVolume * Data.MasterVolume;
        SoundManager.source.volume = Data.SoundFXVolume * Data.MasterVolume;
    }

}