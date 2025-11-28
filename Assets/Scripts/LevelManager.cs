using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;



public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject plus, minus, reset;
    [SerializeField] private GameObject pauseContainer;

    private Animator cameraAnim;
    public static LevelManager Instance;
    private bool isPaused = false;

    private PlayerInputActions input;


    private void Awake()
    {
        Instance = this;
        cameraAnim = Camera.main.GetComponent<Animator>();
        StartCoroutine(nameof(ActivateUI));

    }
    private void OnEnable()
    {
        input = new PlayerInputActions();
        input.Enable();
        input.Player.Pause.performed += PauseGame;
        input.Player.Reset.performed += ResetLevel;
    }

    private IEnumerator ActivateUI()
    {
        minus.SetActive(false);
        plus.SetActive(false);
        reset.SetActive(false);
        yield return new WaitForSeconds(1f);
        minus.SetActive(true);
        plus.SetActive(true);
        reset.SetActive(true);
    }

    private void PauseGame(InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused;
        pauseContainer.SetActive(isPaused);
        // Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResetLevel(InputAction.CallbackContext ctx)
    {
        StartCoroutine(nameof(ResetLevelCoroutine));
    }

    private IEnumerator ResetLevelCoroutine()
    {
        minus.SetActive(false);
        plus.SetActive(false);
        reset.SetActive(false);
        SoundManager.instance.Play("Reset");
        yield return new WaitForSeconds(.1f);
        cameraAnim.SetTrigger("ZoomReset");
        yield return new WaitForSeconds(.8f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().name == "Credits")
            StartCoroutine(nameof(LoadMenu));
        else
            StartCoroutine(nameof(LoadNextLevelCoroutine));
    }

    private IEnumerator LoadNextLevelCoroutine()
    {
        minus.SetActive(false);
        plus.SetActive(false);
        reset.SetActive(false);
        yield return new WaitForSeconds(.1f);
        cameraAnim.SetTrigger("ZoomOut");
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator LoadMenu()
    {
        minus.SetActive(false);
        plus.SetActive(false);
        reset.SetActive(false);
        yield return new WaitForSeconds(.1f);
        cameraAnim.SetTrigger("ZoomOut");
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene("Menu");
    }

    public void ResumeButton()
    {
        PauseGame(new InputAction.CallbackContext());
    }

    public void MenuButton()
    {
        PauseGame(new InputAction.CallbackContext());
        StartCoroutine(nameof(LoadMenu));
    }
}
