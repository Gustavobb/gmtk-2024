using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FM : MonoBehaviour
{
    public int level = 0;

    private void Start()
    {
        int numFM = FindObjectsByType<FM>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length;
        if (numFM != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        LoadData();
    }

    public void SaveData()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentLevelIndex <= level) return;
        level = currentLevelIndex;
        SaveSystem.Save(this);
    }

    public void LoadData()
    {
        Data data = SaveSystem.Load();
        if (data == null)
        {
            level = 0;
        }
        else
        {
            level = data.level;
        }
    }
}
