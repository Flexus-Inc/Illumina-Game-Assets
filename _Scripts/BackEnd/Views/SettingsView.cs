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
    void Start() {
        SettingsController.LoadSettingsData();
        UpdateSliderValues();
        SettingsController.UpdateVolume();
        MusicManager.ChangeMusic(2, true, 1, 1);
    }

    public void UpdateSettings(int type) {
        switch (type) {
            case 1:
                SettingsController.Data.MusicVolume = MusicSlider.value;
                break;
            case 2:
                SettingsController.Data.SoundFXVolume = SoundFXSlider.value;
                break;
            default:
                SettingsController.Data.MasterVolume = MasterSlider.value;
                break;
        }
        SettingsController.UpdateVolume();
        SettingsController.SaveSettingsData();
    }

    public void UpdateSliderValues() {
        MusicSlider.normalizedValue = SettingsController.Data.MusicVolume;
        SoundFXSlider.normalizedValue = SettingsController.Data.SoundFXVolume;
        MasterSlider.normalizedValue = SettingsController.Data.MasterVolume;
    }

}