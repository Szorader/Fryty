using UnityEngine;
using System.Collections;

public class MouseTriggerFloor : MonoBehaviour
{
    public MouseTheThief mouse;

    private Coroutine enterCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fry"))
        {
            // anuluj poprzednie oczekiwanie
            if (enterCoroutine != null)
                StopCoroutine(enterCoroutine);

            enterCoroutine = StartCoroutine(DelayedEnter(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fry"))
        {
            // anuluj wejście jeśli jeszcze nie doszło do reakcji
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
        float delay = Random.Range(3f, 7f);
        yield return new WaitForSeconds(delay);

        mouse.SetTarget(target);
        enterCoroutine = null;
    }
}