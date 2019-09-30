namespace Illumina.Models {

    [System.Serializable]
    public class SettingsData {
        public float MasterVolume = 0.5f;
        public float MusicVolume = 1;
        public float SoundFXVolume = 1;
        public void SetVolumesData(float a, float b, float c) {
            MasterVolume = a;
            MusicVolume = b;
            SoundFXVolume = c;
        }

    }
}