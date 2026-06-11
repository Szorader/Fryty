using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        SaveData data = SaveSystem.Instance.LoadGame();
        
        if (data.sensitivity <= 0)
            data.sensitivity = 1f;

        slider.value = data.sensitivity;

        slider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSensitivityChanged);
    }

    private void OnSensitivityChanged(float value)
    {
        SaveData data = SaveSystem.Instance.LoadGame();

        SaveSystem.Instance.SaveGame(
            data.money,
            data.day,
            data.killedEnemies,
            data.servedClients,
            data.tutorialCompleted,
            data.cleanedTrashCount,
            value
        );
    }
}