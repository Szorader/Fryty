using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

public partial class BasketInteraction : MonoBehaviour
{
    public BasketData basketData;

    [Header("REFERENCES")]
    public CustomerOrder currentCustomer;
    public CustomerWaitingTime waitingTime;
    public CustomerSatisfaction satisfaction;
    public bool isBad;

    public GameObject ketchupBottle;
    public GameObject mayoBottle;
    public GameObject cheeseBottle;
    public GameObject emptySauceBox;

    public GameObject saltShaker;
    public GameObject pepperShaker;

    public GameObject bell;
    
    public QueueManager queueManager;

    public float money = -5f;
    public TMP_Text moneyText;

    private TutorialManager tutorialManager;
    public bool tutorialActive = true;
    public bool tutorialActive2 = true;
    
    [Header("AUDIO")]
    [SerializeField] private EventReference shakerSound;
    [SerializeField] private EventReference sauceSound;
    
    private FMOD.Studio.EventInstance shakerInstance;
    private bool shakerPlaying = false;

    void Start()
    {
        UpdateMoney(0f);
        tutorialManager = FindObjectOfType<TutorialManager>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                HandleClick(hit.collider.gameObject);
            }
        }
    }

    private void HandleClick(GameObject clicked)
    {
        if (!basketData) return;
        if (clicked == ketchupBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.Ketchup);
                Check();
            }
        }
        else if (clicked == mayoBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.Mayo);
                Check();
            }
        }
        else if (clicked == cheeseBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.Cheese);
                Check();
            }
        }
        else if (clicked == emptySauceBox) TrySetSauce(OrderDatabase.SauceType.None, true);

        else if (clicked == saltShaker)
        {
            if (basketData.seasoningType == OrderDatabase.SeasoningType.None)
            {
                //audio
                PlayShakerSound(clicked.transform.position);

                TrySetSeasoning(OrderDatabase.SeasoningType.Salt);
                Check();
            }
        }
        else if (clicked == pepperShaker)
        {
            // audio
            if (basketData.seasoningType == OrderDatabase.SeasoningType.None)
            {
                //audio
                PlayShakerSound(clicked.transform.position);

                TrySetSeasoning(OrderDatabase.SeasoningType.Pepper);
                Check();
            }
        }

        else if (clicked == bell)
        {
            if (tutorialActive2 && tutorialManager.tutorialStep == 5)
            {
                tutorialManager.NextStep();
                tutorialActive2 = false;
            }
            CheckOrder();
            ResetBasket();
        }
    }

    private void Check()
    {
        if  (tutorialActive && tutorialManager.tutorialStep == 4)
        {
            tutorialManager.NextStep();
            tutorialActive = false;
        }
    }
    private void TrySetSauce(OrderDatabase.SauceType newSauce, bool force = false)
    {
        if (basketData.sauceType != OrderDatabase.SauceType.None && !force) return;
        
        basketData.sauceType = newSauce;
        basketData.RefreshVisuals();
    }

    private void TrySetSeasoning(OrderDatabase.SeasoningType newSeasoning)
    {
        if (basketData.seasoningType != OrderDatabase.SeasoningType.None) return;
        
        basketData.seasoningType = newSeasoning;
        basketData.RefreshVisuals();
    }

    private void CheckOrder()
    {
        if (!currentCustomer || !satisfaction || !basketData) return;

        float tip = satisfaction.CalculateTip(
            waitingTime.GetTime(),
            basketData,
            currentCustomer
        );

        Debug.Log("TIP: " + tip);
        if (tip == 0)
        {
            UpdateMoney(tip - 5);
        }
        else
        {
            UpdateMoney(tip);
        }
        
        
        
        queueManager.ServeNextClient();
        
        
    }

    public void UpdateMoney(float amount)
    {
        if (isBad)
        {
            money -= +  15 + amount;
        }
        else
        {
            money += +  5 + amount;
        }
        
        moneyText.text = money.ToString();
    }
    
    public void UpdateMoneyKill(float amount)
    {
        money += amount;
        
        moneyText.text = money.ToString();
    }
    private void ResetBasket()
    {
        basketData.friesType = OrderDatabase.FriesType.None;
        basketData.cookLevel = 0;
        basketData.sauceType = OrderDatabase.SauceType.None;
        basketData.seasoningType = OrderDatabase.SeasoningType.None;

        basketData.RefreshVisuals();
    }
    
    // audio
    private void PlayShakerSound(Vector3 position)
    {
        if (shakerPlaying) return;

        shakerInstance = RuntimeManager.CreateInstance(shakerSound);
        shakerInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        shakerInstance.start();

        shakerPlaying = true;

        StartCoroutine(ReleaseShakerWhenDone());
    }
    
    private IEnumerator ReleaseShakerWhenDone()
    {
        FMOD.Studio.PLAYBACK_STATE state;

        do
        {
            shakerInstance.getPlaybackState(out state);
            yield return null;
        }
        while (state != FMOD.Studio.PLAYBACK_STATE.STOPPED);

        shakerInstance.release();
        shakerPlaying = false;
    }
}