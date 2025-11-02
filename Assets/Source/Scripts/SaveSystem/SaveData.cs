using System;

namespace Assets.Source.Scripts.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public SaveData()
        {
            SelectedLanguage = "English";
        }

        public ScreenSettings ScreenSettings { get; set; }

        public AudioSettings AudioSettings { get; set; }

        public string SelectedLanguage { get; set; }

        public  SensitivitySettings SensitivitySettings { get; set; }
    }

    [Serializable]
    public class SensitivitySettings
    {
        public float X {  get; set; }

        public float Y { get; set; }
    }

    [Serializable]
    public class ScreenSettings
    {
        public int Index { get; set; } = 4;

        public bool IsFullScreen { get; set; } = true;
    }

    [Serializable]
    public class AudioSettings
    {
        public float MasterVolume { get; set; }

        public float EffectsVolume { get; set; }

        public float MusicVolume { get; set; }
    }
}