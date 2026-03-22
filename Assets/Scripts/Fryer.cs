using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fryer : MonoBehaviour
{
    public string requiredTag = "Item";
    public float stepDelay = 2f;

    private Dictionary<GameObject, Coroutine> activeProcesses = new Dictionary<GameObject, Coroutine>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(requiredTag)) return;

        Fryable fryable = other.GetComponent<Fryable>();
        if (fryable == null) return;

        if (!activeProcesses.ContainsKey(other.gameObject))
        {
            Coroutine process = StartCoroutine(FryProcess(fryable));
            activeProcesses.Add(other.gameObject, process);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(requiredTag)) return;

        if (activeProcesses.ContainsKey(other.gameObject))
        {
            StopCoroutine(activeProcesses[other.gameObject]);
            activeProcesses.Remove(other.gameObject);
        }
    }

    IEnumerator FryProcess(Fryable fryable)
    {
        while (true)
        {
            yield return new WaitForSeconds(stepDelay);
            
            fryable.IncreaseFryLevel();
            
            if (fryable.fryLevel >= fryable.materials.Length - 1)
                break;
        }

        activeProcesses.Remove(fryable.gameObject);
    }
}