using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public enum MenuView
    {
        Main,
        Game,
        Settings,
        Credits
    }

    [Header("Editor Preview")]
    [SerializeField] private MenuView currentView;

    [Header("Menu Cards")]
    public GameObject mainCard;
    public GameObject gameCard;
    public GameObject settingsCard;
    public GameObject creditsCard;

    [Header("Game")]
    public string GameSceneName = "Game";

    void Start()
    {
        ApplyView(currentView);
    }

    void DisableAll()
    {
        if (mainCard) mainCard.SetActive(false);
        if (gameCard) gameCard.SetActive(false);
        if (settingsCard) settingsCard.SetActive(false);
        if (creditsCard) creditsCard.SetActive(false);
    }

    void ApplyView(MenuView view)
    {
        DisableAll();

        switch (view)
        {
            case MenuView.Main:
                if (mainCard) mainCard.SetActive(true);
                break;
            case MenuView.Game:
                if (gameCard) gameCard.SetActive(true);
                break;
            case MenuView.Settings:
                if (settingsCard) settingsCard.SetActive(true);
                break;
            case MenuView.Credits:
                if (creditsCard) creditsCard.SetActive(true);
                break;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            ApplyView(currentView);
        }
    }
#endif

    // UI Buttons
    public void _ShowMain() => ApplyView(MenuView.Main);
    public void _ShowGame() => ApplyView(MenuView.Game);
    public void _ShowSettings() => ApplyView(MenuView.Settings);
    public void _ShowCredits() => ApplyView(MenuView.Credits);

    public void _PlayGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    public void _QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}