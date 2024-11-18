using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Button[] levels;

    // Start is called before the first frame update
    void Start()
    {
        levels = GetComponentsInChildren<Button>();
        
        // For each level button
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].interactable = GameManager.Instance.saveData.levels[i].available;
        }
    }

    public void LoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
