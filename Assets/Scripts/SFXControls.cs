using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXControls : MonoBehaviour
{
    public AudioSource audioSourcesstatic;
    public List<AudioSource> audioSources = new List<AudioSource>();
    public GameObject menu, options;
    public bool isMusic;

    void Start()
    {
        audioSources.AddRange(GetComponents<AudioSource>());

        foreach (AudioSource audioSource in audioSources)
        {
            PlayerPrefs.GetFloat(isMusic ? "musicVolume" : "SFXVolume", .5f);
        }
    }

    public void AdjustVol(float vol)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if(audioSource != null){
                print("Adjusting volume to: " + vol);
                audioSource.volume = vol;
                PlayerPrefs.SetFloat(isMusic ? "musicVolume" : "SFXVolume", vol);
            }


        }

        PlayerPrefs.SetFloat(isMusic ? "musicVolume" : "SFXVolume", vol);
    }
}
