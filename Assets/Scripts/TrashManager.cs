using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Transform> trashRoots;
    [SerializeField] private DayManager dayManager;

    private bool canCheck = false;
    private bool summaryTriggered = false;

    private void Start()
    {
        StartCoroutine(EnableCheckingNextFrame());
    }

    private IEnumerator EnableCheckingNextFrame()
    {
        yield return null; // stabilizacja sceny
        canCheck = true;
    }

    private void Update()
    {
        if (!canCheck || summaryTriggered)
            return;

        if (dayManager == null)
        {
            //Debug.Log("DayManager NULL");
            return;
        }

        //Debug.Log("CleaningPhase: " + dayManager.isCleaningPhase);

        if (!dayManager.isCleaningPhase)
            return;

        bool clean = IsEverythingClean();

        //Debug.Log("IsEverythingClean: " + clean);

        if (clean)
        {
            //Debug.Log("ALL TRASH CLEAN -> END DAY");
            summaryTriggered = true;
            dayManager.TriggerSummary();
        }
    }

    private bool IsEverythingClean()
    {
        foreach (Transform root in trashRoots)
        {
            if (root == null)
                continue;

            foreach (Transform child in root)
            {
                if (child.gameObject.activeSelf)
                    return false;
            }
        }

        return true;
    }

    public void ResetTrashState()
    {
        summaryTriggered = false;
    }
}