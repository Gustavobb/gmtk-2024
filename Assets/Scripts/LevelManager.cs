using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            ResetLevel();
        }
    }

    private void ResetLevel(){
        SoundManager.instance.Play("Reset");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
