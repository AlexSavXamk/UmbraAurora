using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour{
    
    public static GameplayManager gameplayManager;

    public AudioSource lightnessMusic;
    public AudioSource darknessMusic;

    bool isFadingIntoDarknessMusic = false;

    void Awake()
    {
        gameplayManager = this;
    }

    void Start()
    {
    }

    void Update()
    {
        if(isFadingIntoDarknessMusic)
        {
            darknessMusic.volume += 0.5f * Time.deltaTime;
            lightnessMusic.volume -= 0.5f * Time.deltaTime;
        }else
        {
            darknessMusic.volume -= 0.5f * Time.deltaTime;
            lightnessMusic.volume += 0.5f * Time.deltaTime;
        }
    }

    public void ToggleBetweenDarkAndLightMusic()
    {
        print("Lets go");

        if(isFadingIntoDarknessMusic)
        {
            isFadingIntoDarknessMusic = false;
        }else
        {
            isFadingIntoDarknessMusic = true;
            darknessMusic.Play();
        }
    }
}