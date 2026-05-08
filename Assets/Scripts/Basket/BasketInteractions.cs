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
    
    [Header("SAUCE")]
    public GameObject emptySauceBox;
    public GameObject ketchupBottle;
    public GameObject mayoBottle;
    public GameObject cheeseBottle;
    public GameObject chiliBottle;
    public GameObject oneIslandBottle;
    public GameObject garlicBottle;
    
    [Header("SEASONING")]
    public GameObject saltShaker;
    public GameObject pepperShaker;
    
    [Header("OTHER")]
    public GameObject bell;
    public GameObject trashBin;
    public GameObject trayShelf;
    
    //public QueueManager queueManager;
    public QueuingDevice queuingDevice;

    public float money = -5f;
    public TMP_Text moneyText;
    public TMP_Text orderMoneyText;

    private TutorialManager tutorialManager;
    public bool tutorialActive = true;
    public bool tutorialActive2 = true;
    public bool tutorialActive3 = true;
    
    [Header("AUDIO")]
    [SerializeField] private EventReference shakerSound;
    [SerializeField] private EventReference sauceSound;
    [SerializeField] private EventReference chaChingSound;
    
    private FMOD.Studio.EventInstance shakerInstance;
    private bool shakerPlaying = false;
    
   
    
    void Start()
    {
        UpdateMoney(0f);
        tutorialManager = FindObjectOfType<TutorialManager>();
        queuingDevice = FindObjectOfType<QueuingDevice>();
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
        
        else if (clicked == chiliBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.Chili);
                Check();
            }
        }
        else if (clicked == oneIslandBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.OneIsland);
                Check();
            }
        }
        else if (clicked == garlicBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.Garlic);
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
            if (tutorialActive2 && tutorialManager.tutorialStep == 7)
            {
                tutorialManager.NextStep();
                tutorialActive2 = false;
            }

            ApplyBasketToCustomer(); 

            CheckOrder();
            ResetBasket();
        }
        
        else if (clicked == trashBin)
        {
            ResetBasket();
        }
        
        else if (clicked == trayShelf)
        {
            if (tutorialActive3 && tutorialManager.tutorialStep == 8)
            {
                tutorialManager.NextStep();
                tutorialActive3 = false;
            }
            basketData.trayVisible = true;
            basketData.RefreshVisuals();
        }
    }

    private void Check()
    {
        if  (tutorialActive && tutorialManager.tutorialStep == 6)
        {
            tutorialManager.NextStep();
            tutorialActive = false;
        }
    }
    private void TrySetSauce(OrderDatabase.SauceType newSauce, bool force = false)
    {
        if (!basketData.trayVisible || basketData.friesType == OrderDatabase.FriesType.None)
            return;

        if (basketData.sauceType != OrderDatabase.SauceType.None && !force)
            return;

        basketData.sauceType = newSauce;
        basketData.RefreshVisuals();
    }

    private void TrySetSeasoning(OrderDatabase.SeasoningType newSeasoning)
    {
        if (!basketData.trayVisible || basketData.friesType == OrderDatabase.FriesType.None)
            return;

        if (basketData.seasoningType != OrderDatabase.SeasoningType.None)
            return;

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
        
        // play cha-ching audio
        RuntimeManager.PlayOneShot(chaChingSound, transform.position);
        
        
        //queueManager.ServeNextClient();
        queuingDevice.RemoveClient();
        
        
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
        orderMoneyText.text = amount.ToString();
    }
    
    
    public void UpdateMoneyKill(float amount)
    {
        money += amount;
        
        moneyText.text = money.ToString();
        orderMoneyText.text = amount.ToString();
    }
    private void ResetBasket()
    {
        basketData.friesType = OrderDatabase.FriesType.None;
        basketData.cookLevel = 0;
        basketData.sauceType = OrderDatabase.SauceType.None;
        basketData.seasoningType = OrderDatabase.SeasoningType.None;

        basketData.trayVisible = false;

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
    
    private void ApplyBasketToCustomer()
    {
        if (!basketData || !currentCustomer) return;

        BasketData customerBasket = currentCustomer.GetComponent<BasketData>();
        Animator anim = currentCustomer.GetComponent<Animator>();

        if (!customerBasket)
        {
            Debug.LogWarning("Client has no BasketData!");
            return;
        }

        // COPY DATA
        customerBasket.friesType = basketData.friesType;
        customerBasket.cookLevel = basketData.cookLevel;
        customerBasket.sauceType = basketData.sauceType;
        customerBasket.seasoningType = basketData.seasoningType;

        // VISUAL
        customerBasket.trayVisible = true;
        customerBasket.RefreshVisuals();

        // ANIMATION FLAG
        bool hasFries = basketData.friesType != OrderDatabase.FriesType.None;

        if (anim)
        {
            anim.SetBool("hasFries", hasFries);
        }
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