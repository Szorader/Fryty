using System.Collections;
using UnityEngine;
using TMPro;


// system portfela w krórym niżej opisane funkcje wykonują operacje na koncie gracza
public class Wallet : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text operationText;
    [SerializeField] private TMP_Text tipText;

    [Header("Money")]
    [SerializeField] private float balance = 0f;
    
    [Header("Error UI")]
    [SerializeField] private TMP_Text errorText;
    private Coroutine errorCoroutine;

    private Coroutine operationCoroutine;
    private Coroutine tipCoroutine;

    private void Start()
    {
        if (operationText != null)
            operationText.gameObject.SetActive(false);

        if (tipText != null)
            tipText.gameObject.SetActive(false);

        LoadBalanceFromSave();

        if (balance < 0f)
        {
            DeathScreenManager deathScreen =
                FindObjectOfType<DeathScreenManager>();

            if (deathScreen != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                deathScreen.ShowBankrupt(player);
            }
        }

        RefreshUI();
    }

    // zarobek 
    public void EarnMoney(float amount)
    {
        if (amount == 0f) return;

        ShowOperation($"+{amount:0.00}");

        StartCoroutine(ApplyAfterDelay(amount));

        RefreshUI();
    }
    
    // napiwek
    public void AddTip(float amount)
    {
        if (amount == 0f) return;

        ShowTip($"+{amount:0.00}");

        StartCoroutine(ApplyAfterDelay(amount));

        RefreshUI();
    }
    
    // wydatek
    public void SpendMoney(float amount)
    {
        if (amount == 0f) return;

        ShowOperation($"-{amount:0.00}");

        StartCoroutine(ApplyAfterDelay(-amount));

        RefreshUI();
    }
    
    // aktualne saldo
    public float GetBalance()
    {
        return balance;
    }

    private IEnumerator ApplyAfterDelay(float amount)
    {
        balance += amount;

        SaveBalanceToSave();

        StartCoroutine(RefreshUIAfterDelay());

        yield break;
    }
    
    private IEnumerator RefreshUIAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (balanceText != null)
            balanceText.text = balance.ToString("0.00");
    }

    private void ShowOperation(string text)
    {
        if (operationText == null) return;

        operationText.gameObject.SetActive(true);
        operationText.text = text;

        if (operationCoroutine != null)
            StopCoroutine(operationCoroutine);

        operationCoroutine = StartCoroutine(HideAfterTime(operationText.gameObject, 3f));
    }

    private void ShowTip(string text)
    {
        if (tipText == null) return;

        tipText.gameObject.SetActive(true);
        tipText.text = text;

        if (tipCoroutine != null)
            StopCoroutine(tipCoroutine);

        tipCoroutine = StartCoroutine(HideAfterTime(tipText.gameObject, 3f));
    }

    private IEnumerator HideAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
    
    private void LoadBalanceFromSave()
    {
        if (SaveSystem.Instance == null) return;
        balance = SaveSystem.Instance.LoadGame().money;
    }
    private void SaveBalanceToSave()
    {
        if (SaveSystem.Instance == null) return;
        
        if (SaveSystem.Instance.saveData == null)
            SaveSystem.Instance.saveData = new SaveData();
        
        SaveSystem.Instance.saveData.money = balance;
    }
    
    public bool HasMoney(float amount)
    {
        return balance >= amount;
    }
    
    public void ShowError(string text)
    {
        if (errorText == null) return;

        errorText.gameObject.SetActive(true);
        errorText.text = text;

        if (errorCoroutine != null)
            StopCoroutine(errorCoroutine);

        errorCoroutine = StartCoroutine(HideAfterTime(errorText.gameObject, 2f));
    }
}