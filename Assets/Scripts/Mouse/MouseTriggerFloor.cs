using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseTriggerFloor : MonoBehaviour
{
    public MouseTheThief mouse;

    private Queue<Transform> friesQueue = new Queue<Transform>();
    private Coroutine tickCoroutine;

    [SerializeField] private float sendInterval = 5f;

    void Start()
    {
        tickCoroutine = StartCoroutine(SendLoop());
    }

    private void OnDestroy()
    {
        if (tickCoroutine != null)
            StopCoroutine(tickCoroutine);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fry")) return;

        Transform fry = other.transform;

        if (!friesQueue.Contains(fry))
            friesQueue.Enqueue(fry);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Fry")) return;

        Transform fry = other.transform;

        RemoveFromQueue(fry);

        if (mouse != null)
            mouse.ReturnToHome();
    }

    private IEnumerator SendLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(sendInterval);

            if (mouse == null)
                continue;
            
            if (mouse.gameObject.activeInHierarchy)
                continue;

            if (friesQueue.Count == 0)
                continue;

            Transform next = friesQueue.Dequeue();

            if (next == null)
                continue;

            if (!next.gameObject.activeInHierarchy)
                continue;

            mouse.SetTarget(next);
        }
    }

    private void RemoveFromQueue(Transform target)
    {
        if (friesQueue.Count == 0) return;

        Queue<Transform> temp = new Queue<Transform>();

        while (friesQueue.Count > 0)
        {
            var t = friesQueue.Dequeue();
            if (t != target)
                temp.Enqueue(t);
        }

        friesQueue = temp;
    }
}