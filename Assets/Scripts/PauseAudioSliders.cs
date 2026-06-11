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

    private void OnEnable()
    {
        RefreshSlider();

        slider.onValueChanged.RemoveListener(OnValueChanged);
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void RefreshSlider()
    {
        if (VcaController.Instance == null)
            return;

        float value = 1f;

        switch (sliderType)
        {
            case SliderType.Master:
                value = VcaController.Instance.GetMasterVolume();
                break;

            case SliderType.Music:
                value = VcaController.Instance.GetMusicVolume();
                break;

            case SliderType.SFX:
                value = VcaController.Instance.GetSFXVolume();
                break;
        }

        slider.SetValueWithoutNotify(value);
    }

    private void OnValueChanged(float value)
    {
        if (VcaController.Instance == null)
            return;

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