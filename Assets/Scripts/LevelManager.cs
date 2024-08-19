using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject plus, minus, reset;

    private Animator cameraAnim;
    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
        cameraAnim = Camera.main.GetComponent<Animator>();
    }

    private void Awake(){
        StartCoroutine("ActivateUI");
    }

    private IEnumerator ActivateUI(){
        minus.SetActive(false);
        plus.SetActive(false);
        reset.SetActive(false);
        yield return new WaitForSeconds(.8f);
        minus.SetActive(true);
        plus.SetActive(true);
        reset.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            ResetLevel();
        }
    }

    public void ResetLevel(){
        StartCoroutine("ResetLevelCoroutine");
    }

    private IEnumerator ResetLevelCoroutine(){
        minus.SetActive(false);
        plus.SetActive(false);
        reset.SetActive(false);
        yield return new WaitForSeconds(.1f);
        cameraAnim.SetTrigger("ZoomReset");
        yield return new WaitForSeconds(.8f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel(){
        StartCoroutine("LoadNextLevelCoroutine");
    }

    private IEnumerator LoadNextLevelCoroutine(){
        minus.SetActive(false);
        plus.SetActive(false);
        reset.SetActive(false);
        yield return new WaitForSeconds(.1f);
        cameraAnim.SetTrigger("ZoomOut");
        yield return new WaitForSeconds(1.3f);
        if(SceneManager.GetActiveScene().name =="Credits") SceneManager.LoadScene("Menu");
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
