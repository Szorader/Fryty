using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

public partial class BasketInteraction : MonoBehaviour
{
    public BasketData basketData;
    private ClientController currentClientController;

    [Header("REFERENCES")]
    public CustomerOrder currentCustomer;
    public CustomerWaitingTime waitingTime;
    public CustomerSatisfaction satisfaction;

    [Header("WALLET")]
    public Wallet wallet;

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
    
    [Header("DEATH")]
    public DeathScreenManager deathScreenManager;
    public GameObject player;
    public bool isBad;
    
    [Header("OTHER")]

    public GameObject bell;
    public GameObject trashBin;
    public GameObject trayShelf;

    public QueuingDevice queuingDevice;

    public TMP_Text moneyText;
    public TMP_Text orderMoneyText;

    public UI_QueuingDevice queuingDeviceUI;
    public DayManager dayManager;
    private SaveSystem saveSystem;

    [Header("AUDIO")]
    [SerializeField] private EventReference shakerSound;
    [SerializeField] private EventReference sauceSound;
    [SerializeField] private EventReference chaChingSound;

    private FMOD.Studio.EventInstance shakerInstance;
    private bool shakerPlaying = false;
    private bool isLooking = false;

    public GameObject clicked;
    
    public Camera playerCamera;
    public Transform lookTarget;
    public float cameraLookSpeed = 2f;

    void Start()
    {
        queuingDevice = FindObjectOfType<QueuingDevice>();
        saveSystem = FindObjectOfType<SaveSystem>();
        saveSystem.LoadGame();
        dayManager = FindObjectOfType<DayManager>();
    }

    private void Update()
    {
        if (isLooking)
        {
            LookAtTarget();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                HandleClick(hit.collider.gameObject);
            }
        }
        
    }

    void LookAtTarget()
    {
        Vector3 direction = lookTarget.position - playerCamera.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        playerCamera.transform.rotation = Quaternion.Slerp(
            playerCamera.transform.rotation,
            targetRotation,
            cameraLookSpeed * Time.deltaTime
        );
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
        else if (clicked == emptySauceBox)
        {
            TrySetSauce(OrderDatabase.SauceType.None, true);
        }
        else if (clicked == saltShaker)
        {
            if (basketData.seasoningType == OrderDatabase.SeasoningType.None)
            {
                PlayShakerSound(clicked.transform.position);
                TrySetSeasoning(OrderDatabase.SeasoningType.Salt);
            }
        }
        else if (clicked == pepperShaker)
        {
            if (basketData.seasoningType == OrderDatabase.SeasoningType.None)
            {
                PlayShakerSound(clicked.transform.position);
                TrySetSeasoning(OrderDatabase.SeasoningType.Pepper);
            }
        }
        else if (clicked == bell)
        {
            currentClientController = currentCustomer.GetComponent<ClientController>();

            if (currentClientController.isWalking)
                return;
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
            basketData.trayVisible = true;
            basketData.RefreshVisuals();
        }
    }

    private void CheckOrder()
    {
        if (!currentCustomer || !satisfaction || !basketData) return;

        float tip = satisfaction.CalculateTip(
            waitingTime.GetTime(),
            basketData,
            currentCustomer
        );

        FaceController face =
            currentCustomer.GetComponentInChildren<FaceController>();

        if (face != null)
        {
            bool perfect =
                satisfaction.IsPerfectOrder(
                    waitingTime.GetTime(),
                    basketData,
                    currentCustomer
                );

            if (perfect && !isBad) face.PlayTalkingHappy();
            else if (Random.value > 0.5f && !isBad) face.PlayTalkingMad();
            else if (!isBad)face.PlayTalkingSad();
        }

        // WALLET SYSTEM
        if (wallet != null)
        {
            bool hasOrder =
                basketData.friesType != OrderDatabase.FriesType.None;

            if (!hasOrder)
            {
                // puste zamówienie → zero nagrody
                return;
            }

            // brak pieniędzy za surowe frytki
            if (basketData.cookLevel != 0)
            {
                wallet.EarnMoney(4.50f); // base payment
                wallet.AddTip(tip);      // tip
            }    // tip
        }

        dayManager.servedClients++;
        saveSystem.saveData.servedClients = dayManager.servedClients;

        RuntimeManager.PlayOneShot(chaChingSound, transform.position);

        queuingDeviceUI.canGiveOrder = true;
        
        if (isBad)
        {
            isLooking = true;

            player.GetComponent<PlayerMovement>().enabled = false;
            
            Animator anim = currentCustomer.GetComponent<Animator>();

            StartCoroutine(
                EvilCustomerSequence(face, anim)
            );

            return;
        }
        
        StartCoroutine(RemoveCustomerAfterReaction());
    }
    
    // Evil customer eats us
    
    private IEnumerator EvilCustomerSequence(
        FaceController face,
        Animator anim
    )
    {
        Debug.Log("Evil sequence started");

        if (face != null)
        {
            yield return StartCoroutine(
                face.EvilAttack(1.5f)
            );
        }

        Debug.Log("Face finished");

        yield return new WaitForSeconds(0.2f);

        Debug.Log("Starting attack");

        if (anim != null)
        {
            anim.SetBool("isAttacking", true);
        }

        yield return new WaitForSeconds(0.1f);

        if (anim != null)
        {
            anim.SetBool("isAttacking", false);
        }

        yield return new WaitForSeconds(1.2f);

        Debug.Log("Death");

        deathScreenManager.ShowDeath(player);
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

    private void ResetBasket()
    {
        basketData.friesType = OrderDatabase.FriesType.None;
        basketData.cookLevel = 0;
        basketData.sauceType = OrderDatabase.SauceType.None;
        basketData.seasoningType = OrderDatabase.SeasoningType.None;

        StartCoroutine(DisableTrayAfterDelay());
    }

    private void ApplyBasketToCustomer()
    {
        if (!basketData || !currentCustomer) return;

        BasketData customerBasket = currentCustomer.GetComponent<BasketData>();
        Animator anim = currentCustomer.GetComponent<Animator>();

        customerBasket.friesType = basketData.friesType;
        customerBasket.cookLevel = basketData.cookLevel;
        customerBasket.sauceType = basketData.sauceType;
        customerBasket.seasoningType = basketData.seasoningType;

        //customerBasket.trayVisible = true;
        customerBasket.RefreshVisuals();

        if (anim)
            anim.SetBool("hasFries", basketData.friesType != OrderDatabase.FriesType.None);
    }

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

    private IEnumerator RemoveCustomerAfterReaction()
    {
        yield return new WaitForSeconds(2f);
        queuingDevice.RemoveClient();
    }
    
    private IEnumerator DeathAfterAttack()
    {
        yield return new WaitForSeconds(1f); // czas animacji ataku

        deathScreenManager.ShowDeath(player);
    }
    
    private void FacePlayer()
    {
        if (currentCustomer == null || player == null) return;

        Vector3 dir = player.transform.position - currentCustomer.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        currentCustomer.transform.rotation = rot;
    }
    
    private IEnumerator DisableTrayAfterDelay()
    {
        yield return new WaitForSeconds(0.6f);

        basketData.trayVisible = false;
        basketData.RefreshVisuals();
    }
    
    
}