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

    void Start()
    {
        messagePanel.SetActive(false);
        endgame = FindObjectOfType<Endgame>();
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
        StartCoroutine(Message("End Day", 5f, true));
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