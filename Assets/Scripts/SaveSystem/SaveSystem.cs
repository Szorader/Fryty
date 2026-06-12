using System;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public float money;
    public int day;
    public int killedEnemies;
    public int servedClients;
    public int cleanedTrashCount;
    public bool tutorialCompleted;
    public float sensitivity;
}

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private string savePath;
    
    public SaveData saveData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        /*savePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "save.json"
        );*/
        //savePath = Path.Combine(Application.persistentDataPath, "save.json");
        //savePath = Path.Combine(Directory.GetCurrentDirectory(), "Saves", "save.json");
        //saveData = LoadGame();
        string saveFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Saves"
        );

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        savePath = Path.Combine(saveFolder, "save.json");

        Debug.Log("SAVE PATH = " + savePath);
        //saveData = LoadGame();
    }

    // =========================
    // ZAPIS
    // =========================
    public void SaveGame(float money, int day, int killedEnemies, int servedClients, bool tutorialCompleted, int cleanedTrashCount, float sensitivity)
    {
        Debug.Log("SAVE PATH = " + savePath);
        Debug.Log("Gra zapisana: " + savePath);
        
        SaveData data = new SaveData
        {
            money = money,
            day = day,
            killedEnemies = killedEnemies,
            servedClients = servedClients,
            cleanedTrashCount = cleanedTrashCount,
            tutorialCompleted = tutorialCompleted,
            sensitivity = sensitivity
        };

        saveData = data;
        
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        
    }

    // =========================
    // ODCZYT
    // =========================
    public SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            saveData = new SaveData()
            {
                money = data.money,
                day = data.day,
                killedEnemies = data.killedEnemies,
                servedClients = data.servedClients,
                cleanedTrashCount = data.cleanedTrashCount,
                tutorialCompleted = data.tutorialCompleted,
                sensitivity = data.sensitivity
            };
            Debug.Log("Gra wczytana");
            return data;
        }
        
        
        Debug.LogWarning("Brak pliku zapisu");
        return new SaveData()
            {
                money = 4,
                day = 0,
                killedEnemies = 0,
                servedClients = 0,
                tutorialCompleted = false
            };
    }

    // =========================
    // RESET
    // =========================
    public void ResetStats()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Zapis usunięty");
            new SaveData()
            {
                money = 4,
                day = 0,
                killedEnemies = 0,
                servedClients = 0,
                tutorialCompleted = false
            };
        }
    }

    // =========================
    // SPRAWDZENIE TUTORIALU
    // =========================
    public bool IsTutorialCompleted()
    {
        SaveData data = LoadGame();

        if (data != null)
        {
            return data.tutorialCompleted;
        }

        return false;
    }
}