using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        SaveData data = SaveSystem.Instance.LoadGame();

        float sensitivity = data.sensitivity;

        if (sensitivity <= 0.01)
            sensitivity = 1f;

        slider.value = (sensitivity);

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

        if (playerMovement != null)
            playerMovement.SetSensitivity(value);
    }
}