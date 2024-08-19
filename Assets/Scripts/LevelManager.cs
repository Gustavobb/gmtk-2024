using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{

    private Animator cameraAnim;
    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
        cameraAnim = Camera.main.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            ResetLevel();
        }
    }

    private void ResetLevel(){
        StartCoroutine("ResetLevelCoroutine");
    }

    private IEnumerator ResetLevelCoroutine(){
        yield return new WaitForSeconds(.1f);
        cameraAnim.SetTrigger("ZoomReset");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel(){
        StartCoroutine("LoadNextLevelCoroutine");
    }

    private IEnumerator LoadNextLevelCoroutine(){
        yield return new WaitForSeconds(.1f);
        cameraAnim.SetTrigger("ZoomOut");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
