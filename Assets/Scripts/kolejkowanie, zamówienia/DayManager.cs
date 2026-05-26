using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DayManager : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public GameObject messagePanel;

    private Endgame endgame;
    private BasketInteraction basket;
    private Wallet wallet;

    public bool timeToClean = false;
    public bool summary = false;
    public bool isCleaningPhase = false;

    [Header("SKYBOX")]
    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;

    private SaveSystem saveSystem;

    public int killedEnemies;
    public int servedClients;

    void Start()
    {
        messagePanel.SetActive(false);

        endgame = FindObjectOfType<Endgame>();
        basket = FindObjectOfType<BasketInteraction>();
        wallet = FindObjectOfType<Wallet>();
        saveSystem = FindObjectOfType<SaveSystem>();
    }

    private void Update()
    {
        if (summary)
        {
            EndDay();
            summary = false;
        }
    }

    public void TriggerSummary()
    {
        if (!isCleaningPhase)
            return;

        isCleaningPhase = false;
        EndDay();
    }

    public void WrongKill()
    {
        endgame.StartAnimation();
    }

    public void GoodKill()
    {
        StartCoroutine(Message("Good Elimination!", 3f, false));
        killedEnemies++;
    }

    public void EndDay()
    {
        float earnedToday = wallet != null ? 0f : 0f; 
        StartCoroutine(Message("End Day, you earned today: " + (wallet != null ? "see wallet" : "0"), 5f, true));
    }

    public void Save()
    {
        if (wallet == null) return;

        saveSystem.SaveGame(
            saveSystem.saveData.money + GetWalletBalance(),
            saveSystem.saveData.day + 1,
            saveSystem.saveData.killedEnemies + killedEnemies,
            saveSystem.saveData.servedClients + servedClients,
            saveSystem.saveData.tutorialCompleted
        );
    }

    private float GetWalletBalance()
    {
        // jeśli Wallet ma publiczny getter – docelowo tu powinien być access
        // na razie fallback (do poprawy w Wallet)
        return 0f;
    }

    public void CleanTime()
    {
        isCleaningPhase = true;
        SwitchToNight();

        StartCoroutine(Message(
            "Time to start cleaning the outdoor tables",
            3f,
            false
        ));
    }

    IEnumerator Message(string text, float time, bool nextDay)
    {
        messagePanel.SetActive(true);
        messageText.text = text;

        yield return new WaitForSeconds(time);

        messagePanel.SetActive(false);

        if (nextDay)
        {
            saveSystem.saveData.day += 1;
            SceneManager.LoadScene(1);
        }
    }

    public void SwitchToNight()
    {
        RenderSettings.skybox = nightSkybox;
        DynamicGI.UpdateEnvironment();
    }
}