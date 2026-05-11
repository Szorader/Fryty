using System;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject uiTutorial;
    public TextMeshProUGUI tutorialText;


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

    
    
    public int tutorialStep = 0;
    
    //position note in rack to check
    private float targetY = 2.63f;
    private float targetZ = -29.94f;

    private string t;
    private GameObject obj;
    private BasketInteraction bInteraction;


    void Start()
    {
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
                    t = "Place order note on the order rack above the window";
                    Text(t, rack);
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
                        
                    Debug.Log(obj.transform.position + " " + y + " " + z);
                    // check position
                    Debug.Log(y + " " + z + " " + targetY + " " + targetZ);
                    if (y == targetY && z == targetZ)
                    {
                        t = "Give the customer a pager";
                        Text(t, pager);
                    }
                }
                break;
            
            //sprawdzenie dania numerka -> lodówka ziemniak
            case 3:
                obj = GameObject.Find("t_pager");
                if (obj == null)
                {
                    t = "Take a potato out of the fridge";
                    Text(t, fridge);
                }
                break;
            //ziemniak -> krajalnicy
           case 4:
               obj = GameObject.Find("Potato1(Clone)");
               if (obj != null)
               {
                   t = "Put the potato into the slicer on the counter and choose the fries type";
                   Text(t, slicer);
               }
                break;
           //frytki -> smażenie
           case 5:
                obj = GameObject.Find("FRYTUNIE(Clone)");
                if (obj != null)
                {
                    t = "Put the fries into the fryer. Take them out once they're fried";
                    Text(t, fryer);
                }
                break;
           //usmażone -> koszyczek
           case 6:
               obj = GameObject.Find("FRYTUNIE(Clone)");
               FriesData fdata = obj.GetComponent<FriesData>();
               if (fdata.cookDes == "Perfect")
               {
                   t = "Put the fries into the serving basket. Add the correct sauce and spices to the order";
                   Text(t, basket);
                   ketchup.SetActive(true);
                   salt.SetActive(true);
                   bin.SetActive(true);
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
                    t = "Click the correct order number on the register's tablet to summon the customer to the register";
                    Text(t, orderNumber);
                    NumberUI.SetActive(true);
                }
                break;
           //klient idzie -> oddanie zamówienia
           case 8:
               UI_QueuingDevice uiQueuingDevice = NumberUI.GetComponent<UI_QueuingDevice>();
               if (uiQueuingDevice.canGiveOrder == false)
               {
                   t = "Give the order to the customer by clicking on the cash register";
                   Text(t, cashRegister);
               }
               break;
           //oddanie zamówienia -> nowy koszczek
            
            case 9:
                bInteraction = basket.GetComponent<BasketInteraction>();
                
                if (bInteraction.clicked == bInteraction.bell)
                {
                    t = "Add clear basket";
                    Text(t, stand); 
                }
                break;
            //nowy koszyczek -> zabicie zlego klienta
            case 10:
                bInteraction = basket.GetComponent<BasketInteraction>();
                if (bInteraction.clicked == bInteraction.trayShelf)
                {
                    t = "Your next client is the bad customer! Pull the chain to kill him";
                    Text(t, chain);
                }
                
                
                break;
        }
        
        
    }

    private void Text(string text, GameObject obj)
    {
        //dzwiek
        //RuntimeManager.PlayOneShot(firstKnock, obj.transform.position);
        tutorialText.text = text;
        obj.SetActive(true);
        tutorialStep++;
    }
}
