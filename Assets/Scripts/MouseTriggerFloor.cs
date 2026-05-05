using UnityEngine;
using System.Collections;

public class MouseTriggerFloor : MonoBehaviour
{
    public MouseTheThief mouse;
    public float enterDelay = 0.5f;

    private Coroutine enterCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fry"))
        {
            // jeśli coś już czeka, anuluj poprzednie wejście
            if (enterCoroutine != null)
                StopCoroutine(enterCoroutine);

            enterCoroutine = StartCoroutine(DelayedEnter(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fry"))
        {
            // jeśli wychodzi zanim zdążymy wejść -> anuluj wejście
            if (enterCoroutine != null)
            {
                StopCoroutine(enterCoroutine);
                enterCoroutine = null;
            }

            mouse.ReturnToHome();
        }
    }

    private IEnumerator DelayedEnter(Transform target)
    {
        yield return new WaitForSeconds(enterDelay);

        mouse.SetTarget(target);
        enterCoroutine = null;
    }
}