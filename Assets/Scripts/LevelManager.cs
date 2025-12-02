using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;



public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject plus, minus, reset;
    [SerializeField] private GameObject pauseContainer;

    public static LevelManager Instance;
    private bool isPaused = false;
    public Material fadeMaterial;

    private PlayerInputActions input;


    private void Awake()
    {
        Instance = this;
        StartCoroutine(Util.AnimateFloat((float val) =>
        {
            fadeMaterial.SetFloat("_Fade", val);
            return 0;
        }, 1f, 0f, 1f, () =>
        {
            minus.SetActive(true);
            plus.SetActive(true);
            reset.SetActive(true);
        }));
    }
    private void OnEnable()
    {
        input = new PlayerInputActions();
        input.Enable();
        input.Player.Pause.performed += PauseGame;
        input.Player.Reset.performed += ResetLevel;
    }

    private void PauseGame(InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused;
        pauseContainer.SetActive(isPaused);
        // Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResetLevel(InputAction.CallbackContext ctx)
    {
        SoundManager.instance.Play("Reset");
        StartCoroutine(Util.AnimateFloat((float val) =>
        {
            fadeMaterial.SetFloat("_Fade", val);
            return 0;
        }, 0f, 1f, 1f, () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }));
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().name == "Credits")
        {
            StartCoroutine(Util.AnimateFloat((float val) =>
            {
                fadeMaterial.SetFloat("_Fade", val);
                return 0;
            }, 0f, 1f, 1f, () =>
            {
                SceneManager.LoadScene("Menu");
            }));
        }
        else
        {
            StartCoroutine(Util.AnimateFloat((float val) =>
            {
                fadeMaterial.SetFloat("_Fade", val);
                return 0;
            }, 0f, 1f, 1f, () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }));
        }
    }

    public void ResumeButton()
    {
        PauseGame(new InputAction.CallbackContext());
    }

    public void MenuButton()
    {
        PauseGame(new InputAction.CallbackContext());
        StartCoroutine(Util.AnimateFloat((float val) =>
        {
            fadeMaterial.SetFloat("_Fade", val);
            return 0;
        }, 0f, 1f, 1f, () =>
        {
            SceneManager.LoadScene("Menu");
        }));
    }
}
