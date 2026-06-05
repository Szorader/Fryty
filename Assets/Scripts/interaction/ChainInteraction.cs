using UnityEngine;
using FMODUnity;
using System.Collections;

/// <summary>
/// Kill clients
/// </summary>
public class ChainInteraction : MonoBehaviour, IInteractable
{
    public string prompt;

    public QueuingDevice queuingDevice;
    public QueueType queueType;
    public DoorReset doorReset;

    [Header("WALLET")]
    public Wallet wallet;

    [Header("REWARD SETTINGS")]
    public float badClientReward = 20f;
    public float goodClientPenalty = 0f;

    [Header("AUDIO")]
    [SerializeField] private EventReference killSound;
    [SerializeField] private EventReference chainSound;
    [SerializeField] private EventReference hatchSound;

    [Header("VFX")]
    [SerializeField] private GameObject smokeEffect;
    private float smokeDuration = 15f;

    public Animator animator;

    public GameObject modelFries1;
    public GameObject modelFries2;
    public GameObject modelFries3;

    public Transform point;

    private Vector3 startLocalPos;

    public bool canInteract = true;
    public bool secondChain = false;
    public bool tutorial = false;

    public enum QueueType
    {
        Order,
        Pickup
    }

    private void Start()
    {
        startLocalPos = transform.localPosition;
        queuingDevice = FindObjectOfType<QueuingDevice>();
    }

    public bool CanInteract()
    {
        if (!secondChain)
            return canInteract;
        else
            return canInteract && queuingDevice.waitingForTake;
    }

    public bool Interact(Interactor interactor)
    {
        canInteract = false;

        ClientController client = null;

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
            return false;

        StartCoroutine(PullChain());
        StartCoroutine(KillSequence(client));

        return true;
    }

    private IEnumerator KillSequence(ClientController client)
    {
        //resetuje pozycje drzwi do domyslnej
        doorReset.ResetDoor();
        
        animator.SetTrigger("close");

        RuntimeManager.PlayOneShot(chainSound, transform.position);
        RuntimeManager.PlayOneShot(hatchSound, transform.position);

        yield return new WaitForSeconds(1f);

        //blokuje i nie mozna otwierac drzwi
        doorReset.LockDoor();
        
        RuntimeManager.PlayOneShot(killSound, transform.position);

        // WALLET LOGIC
        if (wallet != null)
        {
            if (client.isBadClient)
            {
                wallet.EarnMoney(badClientReward);
            }
        }

        if (client.isBadClient)
        {
            queuingDevice.dayManager.GoodKill();
        }
        else
        {
            queuingDevice.dayManager.WrongKill();
        }

        yield return new WaitForSeconds(10f);

        if (smokeEffect != null)
        {
            GameObject fx = Instantiate(smokeEffect, client.transform.position, Quaternion.identity);
            Instantiate(modelFries1, client.transform.position += new Vector3(0.5f, 0f, 0f), Quaternion.identity);
            Instantiate(modelFries2, client.transform.position += new Vector3(-0.5f, 0f, 0f), Quaternion.identity);
            Instantiate(modelFries3, client.transform.position+= new Vector3(-0.5f, 0f, 0.15f), Quaternion.identity);
            Destroy(fx, smokeDuration);
        }

        queuingDevice.KillClient(client);

        //odblokowuje pod animacje
        doorReset.UnlockDoor();
        
        RuntimeManager.PlayOneShot(hatchSound, transform.position);
        animator.SetTrigger("open");

        canInteract = true;
        tutorial = true;
    }

    public string GetPrompt()
    {
        return prompt;
    }

    private IEnumerator PullChain()
    {
        float durationDown = 0.2f;
        float durationUp = 0.4f;
        float distance = 0.15f;

        Vector3 downPos = startLocalPos + Vector3.down * distance;

        float t = 0;

        while (t < durationDown)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startLocalPos, downPos, t / durationDown);
            yield return null;
        }

        yield return new WaitForSeconds(0.02f);

        t = 0;

        while (t < durationUp)
        {
            t += Time.deltaTime;
            float lerp = 1f - Mathf.Pow(1f - (t / durationUp), 3f);
            transform.localPosition = Vector3.Lerp(downPos, startLocalPos, lerp);
            yield return null;
        }

        transform.localPosition = startLocalPos;
    }
}