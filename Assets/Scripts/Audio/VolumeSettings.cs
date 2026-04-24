using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider MasterSlider;
    
    private void Start()
    {
        LoadVolumeSettings();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("MenuMusic", Mathf.Log10(volume)*20);
        audioMixer.SetFloat("GameMusic", Mathf.Log10(volume)*20);

        PlayerPrefs.SetFloat("MusicVolume", volume);
        //PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume)*20);

        PlayerPrefs.SetFloat("SFXVolume", volume);
        //PlayerPrefs.Save();
    }

    public void SetMasterVolume()
    {
        float volume = MasterSlider.value;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume)*20);

        PlayerPrefs.SetFloat("MasterVolume", volume);
        //PlayerPrefs.Save();
    }

    private void LoadVolumeSettings()
    {

        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicSlider.value);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", SFXSlider.value);
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", MasterSlider.value);

        musicSlider.SetValueWithoutNotify(musicVolume);
        SFXSlider.SetValueWithoutNotify(sfxVolume);
        MasterSlider.SetValueWithoutNotify(masterVolume);

        SetMusicVolume();
        SetSFXVolume();
        SetMasterVolume();

    }

}
