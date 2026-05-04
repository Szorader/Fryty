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

    [System.Serializable]
    public class Card
    {
        public GameObject root;        // obiekt karty (dla porządku w hierarchii)
        public CanvasGroup group;      // do sterowania widocznością
    }

    [Header("Editor Preview")]
    [SerializeField] private MenuView currentView;

    [Header("Menu Cards")]
    public Card mainCard;
    public Card gameCard;
    public Card settingsCard;
    public Card creditsCard;

    [Header("Game")]
    public string GameSceneName = "Game";

    void Start()
    {
        ApplyView(currentView);
    }

    void Hide(Card c)
    {
        if (c == null || c.group == null) return;

        c.group.alpha = 0f;
        c.group.interactable = false;
        c.group.blocksRaycasts = false;
    }

    void Show(Card c)
    {
        if (c == null || c.group == null) return;

        c.group.alpha = 1f;
        c.group.interactable = true;
        c.group.blocksRaycasts = true;
    }

    void DisableAll()
    {
        Hide(mainCard);
        Hide(gameCard);
        Hide(settingsCard);
        Hide(creditsCard);
    }

    void ApplyView(MenuView view)
    {
        DisableAll();

        switch (view)
        {
            case MenuView.Main: Show(mainCard); break;
            case MenuView.Game: Show(gameCard); break;
            case MenuView.Settings: Show(settingsCard); break;
            case MenuView.Credits: Show(creditsCard); break;
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