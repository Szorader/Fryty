using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class VcaController : MonoBehaviour
{
    public static VcaController Instance;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        masterBus = RuntimeManager.GetBus("bus:/_Master");
        musicBus = RuntimeManager.GetBus("bus:/_Master/Music");
        sfxBus = RuntimeManager.GetBus("bus:/_Master/SFX");

        LoadSavedVolumes();
    }

    public void SetMasterVolume(float volume)
    {
        masterBus.setVolume(volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicBus.setVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxBus.setVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat("MasterVolume", 1f);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    private void LoadSavedVolumes()
    {
        SetMasterVolume(GetMasterVolume());
        SetMusicVolume(GetMusicVolume());
        SetSFXVolume(GetSFXVolume());
    }
}
