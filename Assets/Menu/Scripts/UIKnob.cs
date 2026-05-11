using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour, IDragHandler
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
    [SerializeField] private RectTransform handle;
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text valTxt;
    [SerializeField] private RectTransform center;

    [Range(0f, 1f)]
    [SerializeField] private float startValue = 0.5f;

    private float currentValue;

    private void Start()
    {
        // load saved value
        switch (sliderType)
        {
            case SliderType.Master:
                startValue = VcaController.Instance.GetMasterVolume();
                break;

            case SliderType.Music:
                startValue = VcaController.Instance.GetMusicVolume();
                break;

            case SliderType.SFX:
                startValue = VcaController.Instance.GetSFXVolume();
                break;
        }

        SetValue(startValue);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dir = eventData.position - (Vector2)center.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle < 0) ? angle + 360f : angle;

        if (angle <= 225f || angle >= 315f)
        {
            handle.rotation = Quaternion.Euler(0, 0, angle + 135f);

            float normalized =
                (angle >= 315f ? angle - 360f : angle) + 45f;

            float value = 0.75f - (normalized / 360f);

            SetValue(value / 0.75f);
        }
    }

    public void SetValue(float value01)
    {
        value01 = Mathf.Clamp01(value01);
        currentValue = value01;

        float fillValue = value01 * 0.75f;
        fill.fillAmount = fillValue;

        float normalized = (0.75f - fillValue) * 360f;
        float angle = normalized - 45f;

        if (angle < 0)
            angle += 360f;

        handle.rotation =
            Quaternion.Euler(0, 0, angle + 135f);

        valTxt.text =
            Mathf.Round(value01 * 100f).ToString();

        ApplyAudio();
    }

    private void ApplyAudio()
    {
        switch (sliderType)
        {
            case SliderType.Master:
                VcaController.Instance.SetMasterVolume(currentValue);
                break;

            case SliderType.Music:
                VcaController.Instance.SetMusicVolume(currentValue);
                break;

            case SliderType.SFX:
                VcaController.Instance.SetSFXVolume(currentValue);
                break;
        }
    }
}