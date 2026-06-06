using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

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
    
    [Header("Audio")]
    [SerializeField] private EventReference bottle_squirt;
    [SerializeField] private EventReference uiClickSound;
    [SerializeField] private EventReference uiWriteSound;
    
    
    private SaveSystem saveSystem;
    

    void Start()
    {
        ApplyView(currentView);
        saveSystem = FindObjectOfType<SaveSystem>();
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
    public void _ShowMain_Settings() // from SETTINGS
    {
        PlayUISound();
        ApplyView(MenuView.Main);
    }
    public void _ShowMain_Game() // from GAME SELECTION
    {
        PlayUISound();
        ApplyView(MenuView.Main);
    }

    public void _ShowGame()
    {
        PlaySquirtSound();
        ApplyView(MenuView.Game);
    }

    public void _ShowCredits()
    {
        PlayUISound();
        ApplyView(MenuView.Credits);
    }

    // From MAIN MENU 
    public void _ShowSettings_Main()
    {
        PlaySquirtSound();
        ApplyView(MenuView.Settings);
    }

    // From CREDITS (back button)  
    public void _ShowSettings_Back()
    {
        PlayUISound();
        ApplyView(MenuView.Settings);
    }


    public void _PlayNewGame()
    {
        PlayWritingSound();
        saveSystem.ResetStats();
        SceneManager.LoadScene(1);
    }
    public void _PlayGame()
    {
        PlayWritingSound();
        SceneManager.LoadScene(1);
    }

    public void _PlayTutorial()
    {
        PlayWritingSound();
        SceneManager.LoadScene(2);
    }

    public void _QuitGame()
    {
        PlaySquirtSound();
        Debug.Log("Quit Game");
        Application.Quit();
    }
    
    // Audio
    private void PlaySquirtSound()
    {
        RuntimeManager.PlayOneShot(bottle_squirt);
    }
    private void PlayUISound()
    {
        RuntimeManager.PlayOneShot(uiClickSound);
    }

    private void PlayWritingSound()
    {
        RuntimeManager.PlayOneShot(uiWriteSound);
    }
}