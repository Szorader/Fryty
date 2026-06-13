using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using FMODUnity;

public class Tutorial : MonoBehaviour
{
    public GameObject uiTutorial;
    public TextMeshProUGUI tutorialText;
    

    public EventReference tutorialSound;

    public GameObject pager;
    public GameObject rack;
    public GameObject fridge;
    public GameObject slicer;
    public GameObject fryer;
    public GameObject basket;
    public GameObject ketchup;
    public GameObject salt;
    public GameObject bin;
    public GameObject orderNumber;
    public GameObject NumberUI;
    public GameObject cashRegister;
    public GameObject stand;
    public GameObject chain;
    public GameObject broom;
    public GameObject umbrella;
    public GameObject doorlock;

    [Header("Tutorial Arrows")]
    public GameObject pagerArrow;
    public GameObject rackArrow;
    public GameObject fridgeArrow;
    public GameObject slicerArrow;
    public GameObject fryerArrow;
    public GameObject basketArrow;
    public GameObject orderArrow;
    public GameObject cashRegisterArrow;
    public GameObject standArrow;
    public GameObject chainArrow;
    public GameObject broomArrow;
    public GameObject trashArrow;
    public GameObject orderNumberArrow; //on the tablet
    public GameObject endDayArrow;
    
    
    private GameObject currentArrow;
    
    public int tutorialStep = 0;
    
    //position note in rack to check
    private float targetY = 2.63f;
    private float targetZ = -29.94f;

    private string t;
    private GameObject obj;
    private BasketInteraction bInteraction;
    public TutorialSpawner spawner;
    
    private SaveSystem saveSystem;
    public DayManager dayManager;
    


    void Start()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
        dayManager = FindObjectOfType<DayManager>();
        //uiTutorial.SetActive(true);
        t = "Approach the customer and take the order";
        Text(t, uiTutorial);
        
        pager.SetActive(false);
        fridge.SetActive(false);
        rack.SetActive(false);
        slicer.SetActive(false);
        fryer.SetActive(false);
        basket.SetActive(false);
        ketchup.SetActive(false);
        salt.SetActive(false);
        bin.SetActive(false);
        orderNumber.SetActive(false);
        NumberUI.SetActive(false);
        cashRegister.SetActive(false);
        stand.SetActive(false);
        chain.SetActive(false);
        broom.SetActive(false);
        //umbrella.SetActive(false);

        
        
    }
    private void Update()
    {
        switch (tutorialStep)
        {
            //check note ->przypięcie kartki
            case 1:
                obj = GameObject.Find("OrderTicket(Clone)");
                if (obj != null)
                {
                    rackArrow.SetActive(true);
                    t = "Place order note on the order rack above the window";
                    tutorialText.text = t;
                    rack.SetActive(true);

                    tutorialStep++;
                }
                break;
            
            //sprawdznie przyczepienia karteczki -> danie numerka
            case 2:
                obj = GameObject.Find("OrderTicket(Clone)");
                    
                if (obj != null)
                {
                        
                    Vector3 pos = obj.transform.position;
                        
                    // zaokrąglenie
                    float y = Mathf.Round(pos.y * 100f) / 100f;
                    float z = Mathf.Round(pos.z * 100f) / 100f;
                        
                    //Debug.Log(obj.transform.position + " " + y + " " + z);
                    // check position
                    //Debug.Log(y + " " + z + " " + targetY + " " + targetZ);
                    if (y == targetY && z == targetZ)
                    {
                        rackArrow.SetActive(false);
                        t = "Give the customer a pager.";
                        Text(t, pager);
                        pagerArrow.SetActive(true);
                    }
                }
                break;
            
            //sprawdzenie dania numerka -> lodówka ziemniak
            case 3:
                obj = GameObject.Find("t_pager.002");
                if (obj == null)
                {
                    pagerArrow.SetActive(false);
                    t = "Take a potato out of the fridge.";
                    tutorialText.text = t;
                    fridge.SetActive(true);
                    fridgeArrow.SetActive(true);
                    
                    tutorialStep++;
                }
                break;
            //ziemniak -> krajalnicy
           case 4:
               obj = GameObject.Find("Potato1(Clone)");
               if (obj != null)
               {
                   fridgeArrow.SetActive(false);
                   t = "Put the potato into the slicer on the counter and choose the fries type.";
                   Text(t, slicer);
                   slicerArrow.SetActive(true);
               }
                break;
           //frytki -> smażenie
           case 5:
                obj = GameObject.Find("FRYTUNIE(Clone)");
                if (obj != null)
                {
                    slicerArrow.SetActive(false);
                    t = "Put the fries into the fryer. Take them out once they're fried.";
                    Text(t, fryer);
                    pager.SetActive(false);
                    fryerArrow.SetActive(true);
                }
                Debug.Log("przed");
                break;
           //usmażone -> koszyczek
           case 6:
               Debug.Log("w trakcie");
               obj = GameObject.Find("FRYTUNIE(Clone)");
               FriesData fdata = obj.GetComponent<FriesData>();
               if (fdata.cookDes == "Perfect")
               {
                   fryerArrow.SetActive(false);
                   t = "Put the fries into the serving basket. Add the correct sauce and spices to the order.";
                   Text(t, basket);
                   ketchup.SetActive(true);
                   salt.SetActive(true);
                   bin.SetActive(true);
                   basketArrow.SetActive(true);
               }
                break;
           //koszyczek -> przyłowanie klienta
           case 7:
                obj = GameObject.Find("Servingbasket");
                BasketData bdata = obj.GetComponent<BasketData>();
                if (bdata.friesType == OrderDatabase.FriesType.Straight &&
                    bdata.seasoningType == OrderDatabase.SeasoningType.Salt &&
                    bdata.sauceType == OrderDatabase.SauceType.Ketchup)
                {
                    basketArrow.SetActive(false);
                    t = "Click the correct order number on the register's tablet to summon the customer to the register.";
                    Text(t, orderNumber);
                    NumberUI.SetActive(true);
                    orderNumberArrow.SetActive(true);
                }
                break;
           //klient idzie -> oddanie zamówienia
           case 8:
               UI_QueuingDevice uiQueuingDevice = NumberUI.GetComponent<UI_QueuingDevice>();
               if (uiQueuingDevice.canGiveOrder == false)
               {
                   orderNumberArrow.SetActive(false);
                   t = "Give the order to the customer by clicking on the buttons of the cash register.";
                   Text(t, cashRegister);
                   cashRegisterArrow.SetActive(true);
               }
               break;
           //oddanie zamówienia -> nowy koszczek
            
            case 9:
                bInteraction = basket.GetComponent<BasketInteraction>();
                
                if (bInteraction.clicked == bInteraction.bell)
                {
                    cashRegisterArrow.SetActive(false);
                    t = "Add a new basket and tray by clicking the tray holder.";
                    Text(t, stand);
                    standArrow.SetActive(true);
                    spawner.canSpawn = true;
                }
                break;
            //nowy koszyczek -> zabicie zlego klienta
            case 10:
                bInteraction = basket.GetComponent<BasketInteraction>();
                if (bInteraction.clicked == bInteraction.trayShelf)
                {
                    standArrow.SetActive(false);
                    t = "This customer is rotten! End his suffering by pulling the chain above you. It will close the shutter door and pour hot oil on him.";
                    Text(t, chain);
                    chainArrow.SetActive(true);
                }
                break;
            //zabicie
            case 11:
                ChainInteraction chainInteraction = chain.GetComponent<ChainInteraction>();
                if (chainInteraction.tutorial)
                {
                    chainArrow.SetActive(false);
                    t = "Time to clean outside, take the broom and remove the trash on the outside tables.";
                    Text(t, broom);
                    broomArrow.SetActive(true);
                }
                break;
            //podniesienie miotly
            case 12:
                Broom broomScript = broom.GetComponent<Broom>();
                if (broomScript.IsHeld)
                {
                    t = "Click on the trash while holding the broom to remove the trash.";
                    Text(t, umbrella);
                    broomArrow.SetActive(false);
                    trashArrow.SetActive(true);
                    doorlock.SetActive(false);
                    
                }
                break;
            case 13:
                string[] trashNames = {
                    "F_Bottle_001",
                    "F_Bottle_001 (1)",
                    "F_Bottle_001 (2)",
                    "F_Can_001",
                    "F_Can_001 (1)",
                    "F_Can_002",
                    "F_Can_003",
                    "F_Tissue_001",
                    "F_Tissue_001 (1)"
                };

                bool anyActive = false;

                foreach (string name in trashNames)
                {
                    GameObject obj = GameObject.Find(name);

                    if (obj != null && obj.activeInHierarchy)
                    {
                        anyActive = true;
                        break;
                    }
                }

                if (!anyActive)
                {
                    trashArrow.SetActive(false);
                    t = "Time to end the day, click the truck's driver door. The game saves when you end the day.";
                    Text(t, pager);
                    endDayArrow.SetActive(true);
                }

                break;
                /*GameObject trash =  GameObject.Find("Trash4");
                if (trash != null)
                {
                    t = "Time to end the day, click the car door";
                    Text(t, pager);
                }
                break;*/
            case 14:
                GameObject doorLeft = GameObject.Find("F_DoorLeft_001");
                GameObject doorRight = GameObject.Find("F_DoorRight_001");
                
                EndDayInteraction DLinteraction =  doorLeft.GetComponent<EndDayInteraction>();
                EndDayInteraction DRinteraction =  doorRight.GetComponent<EndDayInteraction>();
                if (DLinteraction.clicked || DRinteraction.clicked)
                {
                    endDayArrow.SetActive(false);
                    t = "You completed the tutorial, congratulations!";
                    Text(t, pager);
                    StartCoroutine(WaitCoroutine());
                    
                }
              
                break;
            
        }
        
        
    }
    

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(5f);

        saveSystem.saveData.tutorialCompleted = true;
        dayManager.Save();
        //SceneManager.LoadScene(1);
    }
    private void Text(string text, GameObject obj)
    {
        //dzwiek
        RuntimeManager.PlayOneShot(
            tutorialSound,
            obj.transform.position
        );

        tutorialText.text = text;

        obj.SetActive(true);
        
        tutorialStep++;
        
    }
    
    private void ShowArrow(GameObject arrow)
    {
        if (currentArrow != null)
            currentArrow.SetActive(false);

        currentArrow = arrow;

        if (currentArrow != null)
            currentArrow.SetActive(true);
    }
}
