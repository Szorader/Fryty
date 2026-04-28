using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour, IDragHandler
{
    [SerializeField] private RectTransform handle;
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text valTxt;
    [SerializeField] private RectTransform center;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dir = eventData.position - (Vector2)center.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle < 0) ? angle + 360f : angle;

        if (angle <= 225f || angle >= 315f)
        {
            handle.rotation = Quaternion.Euler(0, 0, angle + 135f);

            float normalized = (angle >= 315f ? angle - 360f : angle) + 45f;

            float value = 0.75f - (normalized / 360f);
            fill.fillAmount = value;

            valTxt.text = Mathf.Round((value / 0.75f) * 100f).ToString();
        }
    }
}