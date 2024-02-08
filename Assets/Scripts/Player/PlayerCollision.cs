using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Collision : MonoBehaviour
{
    public Canvas CanvasMiniGame;
    public Canvas CanvasAuch;
    public static bool GameIsPaused = false;
    public GameObject MainCamera;
    public GameObject MiniGameCamera;
    private GameObject objectColission;
    private Vector3 targetCanvaPosition = new Vector3(1.9f, 4.7f, 7f);
    private Vector3 targetNextPosition = new Vector3(0f, 1.3f, -48f);
    public int lifesPlayer = 3;
    public GameObject txtMal; 
    public GameObject txtBien;
    private float pauseTime = 2f;
    void Start()
    {
        MiniGameCamera.SetActive(false);
        CanvasMiniGame.gameObject.SetActive(false);
    }

    void Update()
    {
        if (FinishGame())
        {
            Resume();
            ChangeMainCamera();
            objectColission = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BlueGarbage" || other.tag == "RedGarbage" || other.tag == "GreenGarbage"
        || other.tag == "YellowGarbage")
        {
            if (!GameIsPaused)
            {
                Pause();
                ChangeMiniGameCamera();
                objectColission = other.gameObject;
                other.transform.position = targetCanvaPosition;
            }
        } else {
            if (other.tag == "Garbage")
            {
                lifesPlayer -= 1;
                StartCoroutine(PauseForSeconds(2f));
                if (lifesPlayer == 0) {
                    LoadLostScene();
                }
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
        MainCamera.SetActive(!MainCamera.activeSelf);
        MiniGameCamera.SetActive(!MiniGameCamera.activeSelf);
    }

    void ChangeMainCamera()
    {
        MiniGameCamera.SetActive(false);
        MainCamera.SetActive(true);
    }

    bool FinishGame()
    {
        if (objectColission.tag == "BlueGarbage" && Input.GetKeyDown(KeyCode.A))
        {
            txtBien.gameObject.SetActive(true);
            objectColission.transform.position = targetNextPosition;
            return true;
        }

        if (objectColission.tag == "RedGarbage" && Input.GetKeyDown(KeyCode.F))
        {
            txtBien.gameObject.SetActive(true);
            objectColission.transform.position = targetNextPosition;
            return true;
        }

        if (objectColission.tag == "GreenGarbage" && Input.GetKeyDown(KeyCode.S))
        {
            txtBien.gameObject.SetActive(true);
            objectColission.transform.position = targetNextPosition;
            return true;
        }

        if (objectColission.tag == "YellowGarbage" && Input.GetKeyDown(KeyCode.D))
        {
            txtBien.gameObject.SetActive(true);
            objectColission.transform.position = targetNextPosition;
            return true;
        }
        return false;
    }

    void LoadLostScene()
    {
        SceneManager.LoadScene("Lost");
    }

    IEnumerator PauseForSeconds(float seconds)
    {
        CanvasAuch.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        CanvasAuch.gameObject.SetActive(false);
    }
    
}
