using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private LevelSelectButton[] levelButtons;
    private FM FM;
    private void Start()
    {
        int unlockedLevel = FM.level;
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i <= unlockedLevel)
            {
                levelButtons[i].SetButtonColor(true);
            }
            else
            {
                levelButtons[i].SetButtonColor(false);
            }
        }
    }

    private void OnEnable()
    {
        FM = FindFirstObjectByType<FM>();
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }
}