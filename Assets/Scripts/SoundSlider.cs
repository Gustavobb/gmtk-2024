using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slider_music : MonoBehaviour
{
    private Slider slider;
    public GameObject source;
    public bool isMusic;

    void Start()
    {
        source = isMusic ? GameObject.Find("Music") : GameObject.Find("SoundManager");  
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat(isMusic ? "musicVolume" : "SFXVolume", .5f);

        if (slider != null)
        {
            slider.onValueChanged.AddListener(value => source.GetComponent<SFXControls>().AdjustVol(slider.value));
        }
        else
        {
            Debug.LogError("erro");
        }
    }
}
