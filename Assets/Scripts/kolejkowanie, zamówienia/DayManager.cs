using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class DayManager : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public GameObject messagePanel;
    
    private Endgame endgame;
    private BasketInteraction basket;

    public bool timeToClean = false;
    public bool summary = false;
    public bool isCleaningPhase = false;
    
    [Header("SKYBOX")]
    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;

    public int day;

    void Start()
    {
        messagePanel.SetActive(false);
        endgame = FindObjectOfType<Endgame>();
        basket = FindObjectOfType<BasketInteraction>();
        
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
        //StartCoroutine(Message("You kill good guy", 5f, false));
        endgame.StartAnimation();
    }

    public void GoodKill()
    {
        StartCoroutine(Message("Good Elimination!", 3f, false));
    }

    public void EndDay()
    {
        StartCoroutine(Message("End Day, you earned today: " + basket.money, 5f, true));
    }

    public void CleanTime()
    {
        isCleaningPhase = true;
        SwitchToNight(); // zmiana nieba na wieczorne
        StartCoroutine(Message("Time to start cleaning the outdoor tables", 3f, false));
    }
    IEnumerator Message(string tekst, float czas, bool nextDay)
    {
        messagePanel.SetActive(true);
        messageText.text = tekst;

        yield return new WaitForSeconds(czas);

        messagePanel.SetActive(false);
        if (nextDay)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }
    
    // zmiana skyboxa na wieczorny
    public void SwitchToNight()
    {
        RenderSettings.skybox = nightSkybox;

        // refresh lighting
        DynamicGI.UpdateEnvironment();
    }
}