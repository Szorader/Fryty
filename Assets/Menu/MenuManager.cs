using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Cards")]
    public GameObject mainCard;
    public GameObject gameCard;
    public GameObject settingsCard;
    public GameObject creditsCard;
    
    [Header("Game")]
    public string GameSceneName = "Game";

    void Start() { DisableAll(); _ShowMain(); }
    
    void DisableAll()
    {
        mainCard.SetActive(false);
        gameCard.SetActive(false);
        settingsCard.SetActive(false);
        creditsCard.SetActive(false);
    }

    public void _ShowMain() { DisableAll(); mainCard.SetActive(true); }
    public void _ShowGame() { DisableAll(); gameCard.SetActive(true); }
    public void _ShowSettings() { DisableAll(); settingsCard.SetActive(true); }
    public void _ShowCredits() { DisableAll(); creditsCard.SetActive(true); }
    
    public void _PlayGame() {SceneManager.LoadScene(GameSceneName); }
    
    public void _QuitGame() { Debug.Log("Quit Game"); Application.Quit(); }
    
    
    
    
}