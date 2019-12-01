using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    #region Constants

    private const string MUSIC_KEY = "MusicVolume";
    private const string SOUND_EFFECTS_KEY = "SoundEffectsVolume";
    private const int MINIMUM_SOUND_VALUE = -80;
    private const int MAXIMUM_SOUND_VALUE = 0;
    private const float DEFAULT_MUSIC_VOLUME_VALUE = 0.5f;
    private const float DEFAULT_SOUND_EFFECTS_VOLUME_VALUE = 0.5f;

    #endregion

    #region Serialized Fields

    [SerializeField] private AudioMixerGroup _musicAudioMixerGroup;
    [SerializeField] private AudioMixerGroup _soundEffectsAudioMixerGroup;

    #endregion

    #region Methods

    #region Unity Methods

    private void Start()
    {
        SetMusicVolume(GetSavedMusicVolumeValue());
        SetSoundEffectsVolume(GetSavedSoundEffectsValue());
    }

    #endregion

    #region Private Methods

    public float GetSavedMusicVolumeValue()
    {
        float savedMusicVolumeValue = PlayerPrefsManager.GetSavedMusicVolume();

        savedMusicVolumeValue = savedMusicVolumeValue == PlayerPrefsManager.UNSAVED_VOLUME_VALUE
            ? DEFAULT_MUSIC_VOLUME_VALUE
            : savedMusicVolumeValue;

        return savedMusicVolumeValue;
    }

    public float GetSavedSoundEffectsValue()
    {
        float savedSoundEffectsVolumeValue = PlayerPrefsManager.GetSavedSoundEffectsVolume();

        savedSoundEffectsVolumeValue = savedSoundEffectsVolumeValue == PlayerPrefsManager.UNSAVED_VOLUME_VALUE
            ? DEFAULT_SOUND_EFFECTS_VOLUME_VALUE
            : savedSoundEffectsVolumeValue;

        return savedSoundEffectsVolumeValue;
    }

    private float GetCorrectVolumeValue(float sliderValue)
    {
        return MINIMUM_SOUND_VALUE + ((MAXIMUM_SOUND_VALUE - MINIMUM_SOUND_VALUE) * sliderValue);
    }

    #endregion

    #region Public Methods

    public void SetMusicVolume(float volume)
    {
        float value = GetCorrectVolumeValue(volume);
        _musicAudioMixerGroup.audioMixer.SetFloat(MUSIC_KEY, value);

    }

    public void SetSoundEffectsVolume(float volume)
    {
        float value = GetCorrectVolumeValue(volume);
        _soundEffectsAudioMixerGroup.audioMixer.SetFloat(SOUND_EFFECTS_KEY, value);
    }

    #endregion

    #endregion
}