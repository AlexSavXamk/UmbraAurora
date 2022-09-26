using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
    
    public static GameManager gm;

    public GameObject pauseMenu;
    public bool isGamePaused = false;
    
    void Awake()
    {
        gm = this;
    }

    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if(isGamePaused)
        {
            isGamePaused = false;
            pauseMenu.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }else
        {
            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            isGamePaused = true;
            pauseMenu.SetActive(true);
        }
    }
}
