using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Set the GameManager as a Singleton Pattern
    public static GameManager Instance { get; private set; }
    public SaveData saveData;

    public int numberOfLevels;
    public int firstSceneBuildIndex = 1;

    // Identify if a GameManager instance has already been assigned. If it has, destroy this instance.
    private void Awake()
    {
        if (Instance != null && Instance != this)
        { Destroy(this.gameObject); }
        else
        { Instance = this; }

        DontDestroyOnLoad(this.gameObject);
        saveData = new SaveData();
        LoadGame();
    }

    public void CreateNewSave()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/data.json")) 
        { 
            System.IO.File.Delete(Application.persistentDataPath + "/data.json");
            Debug.Log("Deleted Old Save");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        saveData.levels = new Stage[numberOfLevels];
        
        for (int i  = 0; i < numberOfLevels; i++)
        {
            saveData.levels[i] = new Stage();
            saveData.levels[i].available = false;
            saveData.levels[i].highScore = 0;
        }

        saveData.levels[0].available = true;

        SaveGame();
    }
    
    public void SaveGame()
    {
        string data = JsonUtility.ToJson(saveData);
        string filePath = Application.persistentDataPath + "/data.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, data);
        Debug.Log("Save Complete");
    }

    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + "/data.json";
        if (!System.IO.File.Exists(filePath)) { CreateNewSave(); }
        string data = System.IO.File.ReadAllText(filePath);

        saveData = JsonUtility.FromJson<SaveData>(data);
        Debug.Log("Load Complete");
    }
}

[System.Serializable]
public class Stage
{
    public bool available;
    public int highScore;
}
[System.Serializable] public class SaveData
{
    public Stage[] levels;
}
