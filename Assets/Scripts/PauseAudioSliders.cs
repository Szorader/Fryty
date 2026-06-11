using UnityEngine;
using UnityEngine.UI;

public class PauseAudioSliders : MonoBehaviour
{
    public enum SliderType
    {
        Master,
        Music,
        SFX
    }

    [Header("Type")]
    [SerializeField] private SliderType sliderType;

    [Header("UI")]
    [SerializeField] private Slider slider;

    private bool isInitialized;

    private void Start()
    {
        float value = GetSavedValue();

        slider.onValueChanged.RemoveListener(OnValueChanged);

        slider.SetValueWithoutNotify(value);

        slider.onValueChanged.AddListener(OnValueChanged);

        isInitialized = true;
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        if (!isInitialized) return;

        SetVolume(value);
    }

    private float GetSavedValue()
    {
        switch (sliderType)
        {
            case SliderType.Master:
                return VcaController.Instance.GetMasterVolume();

            case SliderType.Music:
                return VcaController.Instance.GetMusicVolume();

            case SliderType.SFX:
                return VcaController.Instance.GetSFXVolume();
        }

        return 1f;
    }

    private void SetVolume(float value)
    {
        switch (sliderType)
        {
            case SliderType.Master:
                VcaController.Instance.SetMasterVolume(value);
                break;

            case SliderType.Music:
                VcaController.Instance.SetMusicVolume(value);
                break;

            case SliderType.SFX:
                VcaController.Instance.SetSFXVolume(value);
                break;
        }
    }
}