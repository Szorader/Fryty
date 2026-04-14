using UnityEngine;

public class CustomerWaitingTime : MonoBehaviour
{
    [Header("Debug Timer (seconds)")] [SerializeField]
    private int timeAlive = 0;

    private float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            timeAlive++;
            timer -= 1f;
        }
    }

    public int GetTime()
    {
        return timeAlive;
    }
}