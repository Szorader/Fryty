using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

// checks if the order was correct
public partial class BasketInteraction : MonoBehaviour
{
    public BasketData basketData;
    private ClientController currentClientController;

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
    public DeathScreenManager deathScreenManager;
    public GameObject player;
    
    public GameObject bell;
    public GameObject trashBin;
    public GameObject trayShelf;
    
    //public QueueManager queueManager;
    public QueuingDevice queuingDevice;

    public float money = -5f;
    public TMP_Text moneyText;
    public TMP_Text orderMoneyText;

    
    public UI_QueuingDevice queuingDeviceUI;
    private DayManager dayManager;
    private SaveSystem saveSystem;
    
    
    [Header("AUDIO")]
    [SerializeField] private EventReference shakerSound;
    [SerializeField] private EventReference sauceSound;
    [SerializeField] private EventReference chaChingSound;
    
    private FMOD.Studio.EventInstance shakerInstance;
    private bool shakerPlaying = false;
    public GameObject clicked;
    
   
    
    void Start()
    {
        UpdateMoney(0f);
        queuingDevice = FindObjectOfType<QueuingDevice>();
        saveSystem = FindObjectOfType<SaveSystem>();
        saveSystem.LoadGame();
        dayManager = FindObjectOfType<DayManager>();
        
        
        moneyText.text = saveSystem.saveData.money.ToString();
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

    private void HandleClick(GameObject click)
    { 
        clicked = click;
        if (!basketData) return;
        if (clicked == ketchupBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.Ketchup);
            }
        }
        else if (clicked == mayoBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.Mayo);
            }
        }
        else if (clicked == cheeseBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.Cheese);
            }
        }
        
        else if (clicked == chiliBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.Chili);
            }
        }
        else if (clicked == oneIslandBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.OneIsland);
            }
        }
        else if (clicked == garlicBottle)
        {
            if (basketData.sauceType == OrderDatabase.SauceType.None)
            {
                RuntimeManager.PlayOneShot(sauceSound, clicked.transform.position);

                TrySetSauce(OrderDatabase.SauceType.Garlic);
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
            }
        }

        else if (clicked == bell)
        {
            clicked = bell;

            currentClientController = currentCustomer.GetComponent<ClientController>();

            //nie mozna dac zamowienia gdy idzie musi sie zatrzymac przy ladzie
            if (currentClientController.isWalking)
                return;
            
            
            Debug.Log("basket");
            ApplyBasketToCustomer(); 

            Debug.Log("basket updated");
            CheckOrder();
            ResetBasket();
        }
        
        else if (clicked == trashBin)
        {
            ResetBasket();
        }
        
        else if (clicked == trayShelf)
        {
            
            basketData.trayVisible = true;
            basketData.RefreshVisuals();
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
        
        // CUSTOMER REACTION
        FaceController face =
            currentCustomer
                .GetComponentInChildren<FaceController>();

        if (face != null)
        {
            bool perfect =
                satisfaction.IsPerfectOrder(
                    waitingTime.GetTime(),
                    basketData,
                    currentCustomer
                );
            
            // if the order was perfect -> happy reaction 
            if (perfect)
            {
                face.PlayTalkingHappy();
            }
            else
            {
                // if the order wasn't perfect, 50% chance to get an angry reaction
                bool angry =
                    Random.value > 0.5f;

                if (angry)
                {
                    face.PlayTalkingMad();
                }
                else // if the order wasn't perfect, 50% chance to get an sad reaction
                {
                    face.PlayTalkingSad();
                }
            }
        }

        Debug.Log("TIP: " + tip);
        if (tip == 0)
        {
            UpdateMoney(tip - 5);
        }
        else
        {
            UpdateMoney(tip);
        }

        dayManager.servedClients++;
        
        // play cha-ching audio
        RuntimeManager.PlayOneShot(chaChingSound, transform.position);

        queuingDeviceUI.canGiveOrder = true;
        //queueManager.ServeNextClient();
        StartCoroutine(RemoveCustomerAfterReaction());
        
        
    }

    public void UpdateMoney(float amount)
    {
        if (isBad)
        {
            deathScreenManager.ShowDeath(player);
            //money -= +  15 + amount;
            
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
    
    // customer walks away after a slight delay
    private IEnumerator RemoveCustomerAfterReaction()
    {
        yield return new WaitForSeconds(2f);

        queuingDevice.RemoveClient();
    }
}