using UnityEngine;
using UnityEngine.UI;

public class Audiochange : MonoBehaviour
{
    public Scrollbar musicScrollbar;
    public Scrollbar sfxScrollbar;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    void Start()
    {

        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1.0f);


        if (musicScrollbar != null)
        {
            musicScrollbar.value = savedMusicVolume;
            AudioManager.instance.MusicVolume(savedMusicVolume);
        }

        if (sfxScrollbar != null)
        {
            sfxScrollbar.value = savedSFXVolume;
            AudioManager.instance.SFXVolume(savedSFXVolume);
        }

        if (musicScrollbar != null)
        {
            musicScrollbar.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (sfxScrollbar != null)
        {
            sfxScrollbar.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
    }

    private void OnMusicVolumeChanged(float value)
    {
        AudioManager.instance.MusicVolume(value);
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        PlayerPrefs.Save();
    }

    private void OnSFXVolumeChanged(float value)
    {
        AudioManager.instance.SFXVolume(value);
        PlayerPrefs.SetFloat(SFXVolumeKey, value);
        PlayerPrefs.Save();
    }
}
