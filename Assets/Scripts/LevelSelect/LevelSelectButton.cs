using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private UnityEngine.UI.Button levelButton;
    [SerializeField] private GameObject lockImage;
    [SerializeField] private GameObject levelText;
    [SerializeField] private Image buttonImage;
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
            levelButton.interactable = true;
            levelText.SetActive(true);
            lockImage.SetActive(false);
            buttonImage.color = unlockedColor;
        }
        else
        {
            levelButton.interactable = false;
            levelText.SetActive(false);
            lockImage.SetActive(true);
            buttonImage.color = lockedColor;
        }
    }
}
