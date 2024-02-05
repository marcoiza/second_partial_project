using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Collision : MonoBehaviour
{
    public Canvas CanvasMiniGame;
    public static bool GameIsPaused = false;
    public Camera MainCamera;
    public Camera MiniGameCamera;
    void Start()
    {
        MiniGameCamera.gameObject.SetActive(false);
        CanvasMiniGame.gameObject.SetActive(false);
    }

    void Update()
    {
        if (FinishGame())
        {
            Resume();
            ChangeMainCamera();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Garbage")
        {
            if (!GameIsPaused)
            {
                Pause();
                ChangeMiniGameCamera();   
            }
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
        CanvasMiniGame.gameObject.SetActive(true);
    }

    private void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        CanvasMiniGame.gameObject.SetActive(false);
    }

    void ChangeMiniGameCamera()
    {
        MainCamera.gameObject.SetActive(!MainCamera.gameObject.activeSelf);
        MiniGameCamera.gameObject.SetActive(!MiniGameCamera.gameObject.activeSelf);
    }

    void ChangeMainCamera()
    {
        MiniGameCamera.gameObject.SetActive(false);
        MainCamera.gameObject.SetActive(true);
    }

    bool FinishGame()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            return true;
        }
        return false;
    }
    
}
