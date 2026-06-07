using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using FMODUnity;

public class DayManager : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public GameObject messagePanel;

    private Endgame endgame;
    private BasketInteraction basket;
    private Wallet wallet;
    public MonoBehaviour playerMovementScript;

    public bool timeToClean = false;
    public bool summary = false;
    public bool isCleaningPhase = false;

    
    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;
    
    private SaveSystem saveSystem;

    public int killedEnemies;
    public int servedClients;
    
    
    [Header("EndGame")]
    public TextMeshProUGUI summaryTMP;
    public GameObject winPanel;
    public Image fadeImage;
    public bool isShowing = false;
    
    [Header("Parasols")]
    [SerializeField] private List<GameObject> openParasols = new List<GameObject>();
    [SerializeField] private List<GameObject> closedParasols = new List<GameObject>();

    [Header("Audio")]
    [SerializeField] private NightAmbienceManager nightAmbienceManager;

    void Start()
    {
        messagePanel.SetActive(false);

        endgame = FindObjectOfType<Endgame>();
        basket = FindObjectOfType<BasketInteraction>();
        wallet = FindObjectOfType<Wallet>();
        saveSystem = FindObjectOfType<SaveSystem>();
    }

    /*private void Update()
    {
        if (summary)
        {
            EndDay();
            summary = false;
        }
    }*/

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
        saveSystem.saveData.killedEnemies = killedEnemies;
    }

    public void EndDay()
    {
        messagePanel.SetActive(false);
        if (saveSystem == null) return;

        SaveData data = saveSystem.saveData;

        //data.killedEnemies += killedEnemies;
        //data.servedClients += servedClients;

        float earnedToday = GetWalletBalance();
        data.money += earnedToday;

        data.day += 1;

        float tax = data.money * 0.19f;
        float total = data.money - tax;

        saveSystem.saveData = data;

        string[] lines =
        {
            $"Day: {data.day}",
            $"Customers served: {data.servedClients}",
            $"Customers killed: {data.killedEnemies}",
            $"Trash cleaned: {data.cleanedTrashCount}",
            $"Money: {data.money:0.00}$",
            $"Tax: -{tax:0.00}$",
            $"Total: {total:0.00}$"
        };

        playerMovementScript.enabled = false;
        isShowing = true;
        winPanel.SetActive(true);
        StartCoroutine(SummarySequence(lines));

        StartCoroutine(FadeSummary());

        saveSystem.SaveGame(
            data.money,
            data.day,
            data.killedEnemies,
            data.servedClients,
            data.tutorialCompleted,
            data.cleanedTrashCount
        );

        killedEnemies = 0;
        servedClients = 0;
    }
    
    public void NextDay()
    {
        Debug.Log("next day");
        if (saveSystem == null) return;
        
        StopAllCoroutines();

        //saveSystem.saveData.day += 1;
        RuntimeManager.GetBus("bus:/").stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        isShowing = false;
        SceneManager.LoadScene(1);
    }
    
    private IEnumerator SummarySequence(string[] lines)
    {
        yield return StartCoroutine(FadeSummary());

        summaryTMP.text = "";

        for (int i = 0; i < lines.Length; i++)
        {
            summaryTMP.text += lines[i] + "\n";
            yield return new WaitForSeconds(0.25f);
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private IEnumerator FadeSummary()
    {
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime;

            float alpha = Mathf.Clamp01(t / 1);

            c.a = alpha;
            fadeImage.color = c;

            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }

    public void Save()
    {
        if (wallet == null) return;
/*
        saveSystem.SaveGame(
            saveSystem.saveData.money + GetWalletBalance(),
            saveSystem.saveData.day + 1,
            saveSystem.saveData.killedEnemies + killedEnemies,
            saveSystem.saveData.servedClients + servedClients,
            saveSystem.saveData.tutorialCompleted
        );*/
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

        if (nightAmbienceManager != null)
            nightAmbienceManager.StartNightAmbience();


        StartCoroutine(Message(
            "Time to start cleaning the outdoor tables",
            3f,
            false
        ));
        
        SetParasolsForCleaning(true);
    }
    
    private void SetParasolsForCleaning(bool cleaning)
    {
        // cleaning = true -> zamykamy otwarte, pokazujemy zamknięte
        foreach (var p in openParasols)
            if (p != null) p.SetActive(!cleaning);

        foreach (var p in closedParasols)
            if (p != null) p.SetActive(cleaning);
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