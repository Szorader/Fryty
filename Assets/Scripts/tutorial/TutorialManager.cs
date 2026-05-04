using UnityEngine;
using TMPro;
using UnityEditor;

public class TutorialManager : MonoBehaviour
{
    public GameObject uiTutorial;
    public TextMeshProUGUI tutorialText;
    public int tutorialStep = 0;
    public int tutorialStepKill = 0;
    public ParticleController client;
    public ParticleController pikacz;
    public ParticleController fridge;
    public ParticleController slicer;
    public ParticleController fryer;
    public ParticleController order;
    public ParticleController cashRegister;
    public ParticleController chain;
    

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

    public void NextStepKill()
    {
        tutorialStepKill++;
        switch (tutorialStepKill)
        {
            case 1:
                tutorialText.text = "Pull the chain to kill the bad customer";
                uiTutorial.SetActive(true);
                chain.TurnOn();
                break;
            case 2:
                if(tutorialStep > 6)
                    uiTutorial.SetActive(false);
                chain.TurnOff();
                UpdateTutorial();
                break;
            
                
            default:
                tutorialText.text = "Error kill 404";
                break;
        }
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
                tutorialText.text = "Give the customer a number ";
                pikacz.TurnOn();
                client.TurnOff();
                break;
            case 2:
                tutorialText.text = "Take potato";
                fridge.TurnOn();
                pikacz.TurnOff();
                break;
            case 3:
                tutorialText.text = "Put the potato into the slicer and press the fries type";
                fridge.TurnOff();
                slicer.TurnOn();
                break;
            case 4:
                tutorialText.text = "Put the slice into the fryer and click after the time to remove the baked";
                slicer.TurnOff();
                fryer.TurnOn();
                break;
            case 5:
                tutorialText.text = "add the sliced meat to the customer's order and choose the sauce and spices";
                fryer.TurnOff();
                order.TurnOn();
                break;
            case 6:
                tutorialText.text = "Give the order to the customer by clicking on the cash register ";
                order.TurnOff();
                cashRegister.TurnOn();
                break;
            case 7:
                cashRegister.TurnOff();
                uiTutorial.SetActive(false);
                break;
            default:
                tutorialText.text = "Error 404";
                break;
        }
    }
    
}
