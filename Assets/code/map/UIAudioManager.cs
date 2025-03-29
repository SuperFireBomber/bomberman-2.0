using UnityEngine;
using UnityEngine.UI;
public class UIAudioManager : MonoBehaviour
{
    public Scrollbar _musicScrollbar, _sfxScrollbar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }
    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }
    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(_musicScrollbar.value);
    }
    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(_sfxScrollbar.value);
    }
}
