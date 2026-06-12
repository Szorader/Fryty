using TMPro;
using UnityEngine;

public class RecipeScore : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        SaveData data = SaveSystem.Instance.LoadGame();

        string tmpScore =
            "----SCORE----\n" +
            $"Days.........{data.day:00}\n" +
            $"cust.served...{data.servedClients:00}\n" +
            $"cust.killed...{data.killedEnemies:00}\n" +
            $"Trash.........{data.cleanedTrashCount:00}\n" +
            "-----------------\n" +
            $"Money....{data.money:0.00}$";

        scoreText.text = tmpScore;
    }
}