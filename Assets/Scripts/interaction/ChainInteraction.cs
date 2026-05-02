using UnityEngine;
using FMODUnity;
using System.Collections;

public class ChainInteraction : MonoBehaviour, IInteractable
{
    public string prompt;

    public QueueManager queueManager;

    public QueueType queueType; // wybierasz w Inspectorze
    
    [Header("AUDIO")]
    [SerializeField] private EventReference killSound;
    
    [Header("VFX")]
    [SerializeField] private GameObject smokeEffect;
    private float smokeDuration = 15f;
    
    public Animator animator;
    
    public GameObject modelFries1;
    public GameObject modelFries2;
    public GameObject modelFries3;

    public Transform point;

    public enum QueueType
    {
        Order,
        Pickup
    }
    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        ClientController client = null;

        // wybór kolejki
        if (queueType == QueueType.Order)
        {
            client = queueManager.MoveFirstClientFromQueue(queueManager.orderQueue);
        }
        else if (queueType == QueueType.Pickup)
        {
            client = queueManager.MoveFirstClientFromQueue(queueManager.pickupQueue);
        }

        if (client == null)
        {
            Debug.Log("Brak klienta");
            return false;
        }
        
        StartCoroutine(KillSequence(client));
        // Audio
        /*RuntimeManager.PlayOneShot(killSound, transform.position);

        // "zabicie" klienta
        //queueManager.RemoveClient(client);
        queueManager.KillClient(client);

        // sprawdzanie czy zły czy dobry
        if (client.isBadClient)
        {
            Debug.Log("Udane! Zabiłeś złego klienta");
            queueManager.dayManager.GoodClient();
            
            queueManager.basket.UpdateMoneyKill(20f);
            
            
        }
        else
        {
            Debug.Log("Zabiłeś dobrego klienta — koniec dnia");
            queueManager.dayManager.WrongClient();
            
            
            // tutaj możesz wywołać np.
            // GameManager.EndDay();
        }
*/
        return true;
    }
    private IEnumerator KillSequence(ClientController client)
    {
        
        animator.SetTrigger("close");

        
        yield return new WaitForSeconds(1f);

        
        RuntimeManager.PlayOneShot(killSound, transform.position);

        
        if (smokeEffect != null)
        {
            GameObject fx = Instantiate(smokeEffect, client.transform.position, Quaternion.identity);
            Instantiate(modelFries1, client.transform.position, Quaternion.identity);
            Instantiate(modelFries2, client.transform.position, Quaternion.identity);
            Instantiate(modelFries3, client.transform.position, Quaternion.identity);
            Destroy(fx, smokeDuration);
        }
        
        
        if (client.isBadClient)
        {
            Debug.Log("Udane! Zabiłeś złego klienta");
            queueManager.dayManager.GoodClient();
            queueManager.basket.UpdateMoneyKill(20f);
        }
        else
        {
            Debug.Log("Zabiłeś dobrego klienta — koniec dnia");
            queueManager.dayManager.WrongClient();
        }

        
        yield return new WaitForSeconds(10f);

        queueManager.KillClient(client);
        animator.SetTrigger("open");
    }

    public string GetPrompt()
    {
        return prompt;
    }
}