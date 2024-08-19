using System;
using UnityEngine.Audio;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound_controller[] sounds;
    public Sound_controller[] musics;
    private AudioSource musicSource;
    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else{
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        foreach (Sound_controller sound in sounds){
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = false;

        }
        musicSource = gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        musicSource.clip = musics[0].clip;
        musicSource.volume = musics[0].volume;
        musicSource.pitch = musics[0].pitch;
        musicSource.loop = musics[0].loop;
        IntroLoop music = new IntroLoop(musicSource,0f, musics[0].clip.length);
        music.start();
    }

    public void Play(string name){
        Sound_controller s = Array.Find(sounds, sound => sound.name == name);
        if (s == null){
            Debug.Log("Som " + name + " errado");
            return;
        }
        Debug.Log("Play Som " + name);
        s.source.Play();
    }

    public void PlayWithSettings(string name, float volume, float pitch, float time = 0){
        Sound_controller s = Array.Find(sounds, sound => sound.name == name);
        if (s == null){
            Debug.Log("Som " + name + " errado");
            return;
        }
        s.source.time = time;
        s.source.volume = volume;
        s.source.pitch = pitch;
        s.source.Play();
    }

    public void Stop(string name){
        Sound_controller s = Array.Find(sounds, sound => sound.name == name);
        if (s == null){
            Debug.Log("Som " + name + " errado");
            return;
        }
        s.source.Stop();
    }

    public void ChangeMusic(string name){
        musicSource = this.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        Sound_controller music = Array.Find(musics, music => music.name == name);
        if (music == null){
            Debug.Log("Som " + name + " errado");
            return;
        }
        AudioClip clip = music.clip;
        musicSource.clip = clip;
        musicSource.Play();
    }

    /* **************************************************** */
    //The Music IntroLoop Class handles looping music, and playing an intro to the loop
    //https://stackoverflow.com/questions/19763532/how-to-have-game-audio-loop-at-a-certain-point
    /*
    //Example of use
    //Construct
    IntroLoop clip = new IntroLoop(audioSource,5f,10f,20f);
    //no intro just loop
    IntroLoop clip2 = new IntroLoop(audioSource,10f,20f,false);

    //you can set it to play once
    clip2.playOnce = true;    

    //call to start
    clip.start();

    //call once a frame, this resets the loop if the time hits the loop boundary
    //or stops playing if playOnce = true
    clip.checkTime(); 

    //call to stop
    clip.stop();
    */

    public class IntroLoop {
        private AudioSource source;
        private float startBoundary;
        private float introBoundary;
        private float loopBoundary;
        //set to play a clip once
        public bool playOnce = false;

        //play from start for intro
        public IntroLoop(AudioSource source, float introBoundary, float loopBoundary) {
            this.source = source;
            this.startBoundary = 0;
            this.introBoundary = introBoundary;
            this.loopBoundary = loopBoundary;
        }
        //play from start for intro or just loop
        public IntroLoop(AudioSource source, float introBoundary, float loopBoundary, bool playIntro) {
            this.source = source;
            this.startBoundary = playIntro?0:introBoundary;
            this.introBoundary = introBoundary;
            this.loopBoundary = loopBoundary;
        }
        //play from startBoundary for intro, then loop
        public IntroLoop(AudioSource source, float startBoundary, float introBoundary, float loopBoundary) {
            this.source = source;
            this.startBoundary = startBoundary;
            this.introBoundary = introBoundary;
            this.loopBoundary = loopBoundary;
        }
        //call to start
        public void start() { this.source.time = this.startBoundary; this.source.Play(); }
        //call every frame
        public void checkTime() {
            Debug.Log(this.source.time);
            if (this.source.time >= this.loopBoundary) {
                if (!this.playOnce) { this.source.time = introBoundary; }
            } 
        }
        //call to stop
        public void stop() { this.source.Stop(); }  
    }
    //The Music IntroLoop Class
    /* **************************************************** */

}
