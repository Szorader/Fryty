using UnityEngine;
using FMODUnity;
using System.Collections;

/// <summary>
/// Kill clients
/// </summary>
public class ChainInteraction : MonoBehaviour, IInteractable
{
    public string prompt;

    //public QueueManager queueManager;
    public QueuingDevice queuingDevice;

    public QueueType queueType; // wybierasz w Inspectorze
    
    [Header("AUDIO")]
    [SerializeField] private EventReference killSound;
    [SerializeField] private EventReference chainSound;
    [SerializeField] private EventReference hatchSound;
    
    [Header("VFX")]
    [SerializeField] private GameObject smokeEffect;
    private float smokeDuration = 15f;
    
    public Animator animator;
    public TutorialManager tutorialManager;
    
    public GameObject modelFries1;
    public GameObject modelFries2;
    public GameObject modelFries3;

    public Transform point;
    
    // chain movement
    private Vector3 startLocalPos;
    
    public bool canInteract = true;
    public bool secondChain = false;
   

    public enum QueueType
    {
        Order,
        Pickup
    }
    
    private void Start()
    {
        startLocalPos = transform.localPosition;
        queuingDevice = FindObjectOfType<QueuingDevice>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        
    }
    public bool CanInteract()
    {
        if (!secondChain)
        {
            if (canInteract)
                return true;
            else
                return false;
        }
        else
        {
            if (canInteract && queuingDevice.waitingForTake)
                return true;
            else
                return false;
            
        }
        
    }

    public bool Interact(Interactor interactor)
    {
        canInteract = false;
        ClientController client = null;

        // wybór kolejki
        if (queueType == QueueType.Order)
        {
            client = queuingDevice.orderQueue.Peek();
            queuingDevice.orderQueue.Dequeue();

        }
        else if (queueType == QueueType.Pickup && queuingDevice.waitingForTake)
        {
            client = queuingDevice.pickList[queuingDevice.currentNumber];
        }

        if (client == null)
        {
            Debug.Log("Brak klienta");
            return false;
        }
        
        StartCoroutine(PullChain());
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
        RuntimeManager.PlayOneShot(chainSound, transform.position);
        RuntimeManager.PlayOneShot(hatchSound, transform.position);
        
        yield return new WaitForSeconds(1f);

        
        RuntimeManager.PlayOneShot(killSound, transform.position);

        
        
        
        
        if (client.isBadClient)
        {
            Debug.Log("Udane! Zabiłeś złego klienta");
            queuingDevice.dayManager.GoodKill();
            queuingDevice.basket.UpdateMoneyKill(20f);
            //queueManager.dayManager.GoodClient();
            //queueManager.basket.UpdateMoneyKill(20f);
        }
        else
        {
            Debug.Log("Zabiłeś dobrego klienta — koniec dnia");
            queuingDevice.dayManager.WrongKill();
            //queueManager.dayManager.WrongClient();
        }

        
        yield return new WaitForSeconds(10f);
        if (smokeEffect != null)
        {
            GameObject fx = Instantiate(smokeEffect, client.transform.position, Quaternion.identity);
            Instantiate(modelFries1, client.transform.position, Quaternion.identity);
            Instantiate(modelFries2, client.transform.position, Quaternion.identity);
            Instantiate(modelFries3, client.transform.position, Quaternion.identity);
            Destroy(fx, smokeDuration);
        }

        //queueManager.KillClient(client);
        queuingDevice.KillClient(client);
        RuntimeManager.PlayOneShot(hatchSound, transform.position);
        animator.SetTrigger("open");
        canInteract = true;
    }

    public string GetPrompt()
    {
        return prompt;
    }
    
    // pull on chain animation
    private IEnumerator PullChain()
    {
        tutorialManager.NextStepKill();
        float durationDown = 0.2f;
        float durationUp = 0.4f;
        float distance = 0.15f; // how far it moves down

        Vector3 downPos = startLocalPos + Vector3.down * distance;

        float t = 0;

        // Move down (fast)
        while (t < durationDown)
        {
            t += Time.deltaTime;
            float lerp = t / durationDown;
            transform.localPosition = Vector3.Lerp(startLocalPos, downPos, lerp);
            yield return null;
        }

        // Optional tiny pause for weight
        yield return new WaitForSeconds(0.02f);

        t = 0;

        // Move up (slower = more natural)
        while (t < durationUp)
        {
            t += Time.deltaTime;
            float lerp = t / durationUp;

            // Ease out (important for feel)
            lerp = 1f - Mathf.Pow(1f - lerp, 3f);

            transform.localPosition = Vector3.Lerp(downPos, startLocalPos, lerp);
            yield return null;
        }

        transform.localPosition = startLocalPos;
    }
}