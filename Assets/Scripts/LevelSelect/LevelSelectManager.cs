using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class LevelSelectManager : MonoBehaviour
{    
    private PlayerInputActions input;
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
        input = new PlayerInputActions();
        FM = FindFirstObjectByType<FM>();
        input.Enable();
        input.UI.Cancel.performed += Back;
    }
    private void OnDisable()
    {
        input.UI.Cancel.performed -= Back;
        input.Disable();
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }

    public void Back(InputAction.CallbackContext ctx)
    {
        print("Going back to main menu");
        SceneManager.LoadScene(0);
    }
    
}