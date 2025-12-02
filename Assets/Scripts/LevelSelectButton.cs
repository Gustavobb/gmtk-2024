using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private int level;

    [SerializeField] private Image ButtonImage;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;

    
    public void LoadLevel()
    {
        SceneManager.LoadScene(level + 1);
    }

    public void SetButtonColor(bool isUnlocked)
    {
        if (isUnlocked)
        {
            ButtonImage.color = unlockedColor;
        }
        else
        {
            ButtonImage.color = lockedColor;
        }
    }
}
