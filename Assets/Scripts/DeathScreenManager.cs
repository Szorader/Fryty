using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using FMODUnity;

public class DeathScreenManager : MonoBehaviour
{
    [Header("UI")]
    public Image fadeImage;
    public TextMeshProUGUI messageText;
    public Button menuButton;

    [Header("Fade")]
    public float fadeDuration = 2f;

    [Header("Destroy Delay")]
    public float deathDelay = 3f;

    public bool isShowing = false;
    
    private DayManager dayManager;

    private void Start()
    {
        dayManager = FindObjectOfType<DayManager>();
    }
    

    public void ShowDeath(GameObject player)
    {
        if (isShowing) return;

        messageText.text = "YOU DIED";
        StartCoroutine(FadeRoutine(player));
    }

    public void ShowArrest(GameObject player)
    {
        if (isShowing) return;

        messageText.text = "YOU WERE ARRESTED";
        StartCoroutine(FadeRoutine(player));
    }

    private IEnumerator FadeRoutine(GameObject player)
    {
        isShowing = true;

        // enable text
        messageText.gameObject.SetActive(true);

        // disable player movement
        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        if (movement != null)
        {
            movement.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Color imageColor = fadeImage.color;
        Color textColor = messageText.color;

        imageColor.a = 0f;
        textColor.a = 0f;

        fadeImage.color = imageColor;
        messageText.color = textColor;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);

            imageColor.a = alpha;
            textColor.a = alpha;

            fadeImage.color = imageColor;
            messageText.color = textColor;

            yield return null;
        }

        yield return new WaitForSeconds(deathDelay);

        menuButton.gameObject.SetActive(true);
    }

    public void ReturnToMenu()
    {
        dayManager.save = false;
        dayManager.EndDay();
        RuntimeManager.GetBus("bus:/").stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Application.LoadLevel ("MainMenu");
    }
    
    public void ShowBankrupt(GameObject player)
    {
        if (isShowing) return;

        messageText.text = "YOU ARE BANKRUPT";
        StartCoroutine(FadeRoutine(player));
    }
}
