using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTrashSpawner : MonoBehaviour
{
    [Header("Trash Parent")]
    [SerializeField] private GameObject trashParent;

    [Header("Settings")]
    [SerializeField] private int minTrash = 1;
    [SerializeField] private int maxTrash = 3;
    [SerializeField] private float requiredSitTime = 5f;

    private List<GameObject> allTrash = new List<GameObject>();
    private List<GameObject> remainingTrash = new List<GameObject>();

    private Dictionary<GameObject, Coroutine> customerTimers = new Dictionary<GameObject, Coroutine>();
    private HashSet<GameObject> validCustomers = new HashSet<GameObject>();

    private void Awake()
    {
        foreach (Transform child in trashParent.transform)
        {
            GameObject trash = child.gameObject;

            allTrash.Add(trash);
            remainingTrash.Add(trash);

            trash.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Customer"))
            return;

        GameObject customer = other.gameObject;
        
        if (customerTimers.ContainsKey(customer))
            return;

        Coroutine timer = StartCoroutine(CustomerStayTimer(customer));
        customerTimers.Add(customer, timer);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Customer"))
            return;

        GameObject customer = other.gameObject;
        
        if (validCustomers.Contains(customer))
        {
            SpawnTrash();
            validCustomers.Remove(customer);
        }
        
        if (customerTimers.ContainsKey(customer))
        {
            StopCoroutine(customerTimers[customer]);
            customerTimers.Remove(customer);
        }
    }

    private IEnumerator CustomerStayTimer(GameObject customer)
    {
        yield return new WaitForSeconds(requiredSitTime);

        if (customer != null)
        {
            validCustomers.Add(customer);
        }
    }

    private void SpawnTrash()
    {
        if (remainingTrash.Count == 0)
            return;

        int amount = Random.Range(minTrash, maxTrash + 1);
        amount = Mathf.Min(amount, remainingTrash.Count);

        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, remainingTrash.Count);

            GameObject selectedTrash = remainingTrash[randomIndex];

            selectedTrash.SetActive(true);

            remainingTrash.RemoveAt(randomIndex);
        }
    }

    public void ClearTrash()
    {
        remainingTrash.Clear();

        foreach (GameObject trash in allTrash)
        {
            trash.SetActive(false);
            remainingTrash.Add(trash);
        }

        validCustomers.Clear();

        foreach (var timer in customerTimers.Values)
        {
            StopCoroutine(timer);
        }

        customerTimers.Clear();
    }
}