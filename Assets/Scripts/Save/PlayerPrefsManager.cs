using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsManager : MonoBehaviour
{
    #region Constants

    private const string PLAYER_PREFS_KEY_START = "Level_";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SOUND_EFFECTS_VOLUME_KEY = "SoundEffect";
    private const string CHOOSED_BACKGROUND_SPRITE_KEY = "ChoosedBackgroundSprite";
    public const float UNSAVED_VOLUME_VALUE = -1f;
    private const float COMPLETED_LEVEL_PERCENT = 1f;

    #endregion

    #region Methods

    public static void SaveStats(float percent, LevelScriptableObj levelScriptableObj)
    {
        PlayerPrefs.SetFloat(PLAYER_PREFS_KEY_START + levelScriptableObj.LevelJson.text.GetHashCode().ToString(),
            percent);
        PlayerPrefs.Save();
    }

    public static void SaveMusicVolume(Slider musicVolumeSlider)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolumeSlider.value);
        PlayerPrefs.Save();
    }

    public static void SaveSoundEffectsVolume(Slider musicVolumeSlider)
    {
        PlayerPrefs.SetFloat(SOUND_EFFECTS_VOLUME_KEY, musicVolumeSlider.value);
        PlayerPrefs.Save();
    }

    public static void SaveChoosedBackgroundSprite(Sprite sprite)
    {
        PlayerPrefs.SetString(CHOOSED_BACKGROUND_SPRITE_KEY, sprite.name);
        PlayerPrefs.Save();
    }

    public static float GetSavedLevelStats(LevelScriptableObj levelScriptableObj)
    {
        string key = PLAYER_PREFS_KEY_START + levelScriptableObj.LevelJson.text.GetHashCode().ToString();
        float temp = 0f;

        if (PlayerPrefs.HasKey(key))
        {
            temp = PlayerPrefs.GetFloat(key);
        }

        return temp;
    }

     /// <summary>
    /// Если пользователь набрал больше чем макс кол-во очков на уровне, то не должно быть прогресса больше 100%
    /// </summary>
    /// <param name="percent"></param>
    public static float GetNormalizedPercent(float percent)
    {
        return percent > COMPLETED_LEVEL_PERCENT ? COMPLETED_LEVEL_PERCENT : percent;
    }

    public static float GetSavedMusicVolume()
    {
        string key = MUSIC_VOLUME_KEY;
        float temp = 0f;

        if (PlayerPrefs.HasKey(key))
        {
            temp = PlayerPrefs.GetFloat(key);
        }
        else
        {
            temp = UNSAVED_VOLUME_VALUE;
        }

        return temp;
    }

    public static float GetSavedSoundEffectsVolume()
    {
        string key = SOUND_EFFECTS_VOLUME_KEY;
        float temp = 0f;

        if (PlayerPrefs.HasKey(key))
        {
            temp = PlayerPrefs.GetFloat(key);
        }
        else
        {
            temp = UNSAVED_VOLUME_VALUE;
        }

        return temp;
    }

    public static string GetSavedBackgroundSpriteName()
    {
        string key = CHOOSED_BACKGROUND_SPRITE_KEY;
        string temp = string.Empty;

        if (PlayerPrefs.HasKey(key))
        {
            temp = PlayerPrefs.GetString(key);
        }

        return temp;
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
    }

    #endregion
}