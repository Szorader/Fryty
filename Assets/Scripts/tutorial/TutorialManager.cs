using UnityEngine;
using TMPro;
using UnityEditor;

public class TutorialManager : MonoBehaviour
{
    public GameObject uiTutorial;
    public TextMeshProUGUI tutorialText;
    public int tutorialStep = 0;
    public ParticleController client;
    public ParticleController fridge;
    public ParticleController slicer;
    public ParticleController fryer;
    public ParticleController order;
    public ParticleController cashRegister;
    

    void Start()
    {
        UpdateTutorial();
    }

    public void NextStep()
    {
        //Debug.Log("NextStep called!\n" + System.Environment.StackTrace);
        tutorialStep++;
        UpdateTutorial();
    }

    private void UpdateTutorial()
    {
        switch (tutorialStep)
        {
            case 0:
                tutorialText.text = "Approach the customer and take the order";
                client.TurnOn();
                break;
            case 1:
                tutorialText.text = "Take potato";
                client.TurnOff();
                fridge.TurnOn();
                break;
            case 2:
                tutorialText.text = "Put the potato into the slicer and press the fries type";
                fridge.TurnOff();
                slicer.TurnOn();
                break;
            case 3:
                tutorialText.text = "Put the slice into the fryer and click after the time to remove the baked";
                slicer.TurnOff();
                fryer.TurnOn();
                break;
            case 4:
                tutorialText.text = "add the sliced meat to the customer's order and choose the sauce and spices";
                fryer.TurnOff();
                order.TurnOn();
                break;
            case 5:
                tutorialText.text = "Give the order to the customer by clicking on the cash register ";
                order.TurnOff();
                cashRegister.TurnOn();
                break;
            case 6:
                cashRegister.TurnOff();
                uiTutorial.SetActive(false);
                break;
            default:
                tutorialText.text = "Error 404";
                break;
        }
    }
    
}
