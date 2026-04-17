using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DayManager : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public GameObject messagePanel;

    void Start()
    {
        messagePanel.SetActive(false);
    }

    public void WrongClient()
    {
        StartCoroutine(Message("Wyeliminowałeś złego klienta!", 5f, true));
    }

    public void GoodClient()
    {
        StartCoroutine(Message("Dobrze wyeliminowałeś!", 3f, false));
    }

    public void EndDay()
    {
        StartCoroutine(Message("Zakończenie dnia", 5f, true));
    }

    IEnumerator Message(string tekst, float czas, bool reset)
    {
        messagePanel.SetActive(true);
        messageText.text = tekst;

        yield return new WaitForSeconds(czas);

        messagePanel.SetActive(false);

        if (reset)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}