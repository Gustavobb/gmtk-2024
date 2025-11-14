using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject plus, minus, reset;
    [SerializeField] private GameObject pauseContainer;

    private Animator cameraAnim;
    public static LevelManager Instance;
    private bool isPaused = false;

    private void Awake()
    {
        Instance = this;
        cameraAnim = Camera.main.GetComponent<Animator>();
        StartCoroutine(nameof(ActivateUI));
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = !isPaused;
        pauseContainer.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResetLevel()
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
        PauseGame();
    }

    public void MenuButton()
    {
        PauseGame();
        StartCoroutine(nameof(LoadMenu));
    }
}
