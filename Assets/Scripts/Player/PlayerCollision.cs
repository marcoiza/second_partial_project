using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Collision : MonoBehaviour
{
    public Canvas CanvasMiniGame;
    public static bool GameIsPaused = false;
    public GameObject MainCamera;
    public GameObject MiniGameCamera;
    private GameObject objectColission;
    private Vector3 targetCanvaPosition = new Vector3(1.9f, 4.7f, 7f);
    private Vector3 targetNextPosition = new Vector3(0f, 1.3f, -48f);
    public int lifesPlayer = 3;
    private int indexHearths = 0;
    public GameObject txtMal;
    public GameObject txtBien;
    public int score = 0;
    public GameObject scoreBox;
    public List<GameObject> gameObjectsToHide = new List<GameObject>();
    public List<GameObject> gameObjectsToHideTwo = new List<GameObject>();
    private bool cameraMiniGameIsActive = false;
    private float miniGameActiveTime; // Tiempo en segundos que la cámara MiniGame está activa
    private float startTime;
    public Image worldImageTime;
    void Start()
    {
        MiniGameCamera.SetActive(false);
        CanvasMiniGame.gameObject.SetActive(false);
        if (scoreBox != null)
        {
            TextMeshProUGUI textMeshPro = scoreBox.GetComponent<TextMeshProUGUI>();

            if (textMeshPro != null)
            {
                textMeshPro.text = "Puntaje: " + score;
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component not found on scoreBox.");
            }
        }
        else
        {
            Debug.LogError("scoreBox is null. Check your initialization.");
        }
        miniGameActiveTime = 5.0f;
    }

    void Update()
    {
        if (cameraMiniGameIsActive)
        {
            float elapsedTime = Time.time - startTime;
            float fillAmount = 1.0f - Mathf.Clamp01(elapsedTime / miniGameActiveTime);
            worldImageTime.fillAmount = fillAmount;
            if ((Time.time - startTime) < miniGameActiveTime)
            {
                if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (!checkCorrectKeyDown())
                    {
                        ToggleVisibilityForSpecificObjectTwo(indexHearths);
                        ToggleVisibilityForSpecificObject(indexHearths);
                        indexHearths += 1;
                        lifesPlayer -= 1;
                        if (lifesPlayer == 0) {
                            LoadLostScene();
                        }
                    }
                    Resume();
                    ChangeMainCamera();
                    objectColission = null;
                }
            }
            else 
            {
                ToggleVisibilityForSpecificObjectTwo(indexHearths);
                ToggleVisibilityForSpecificObject(indexHearths);
                indexHearths += 1;
                lifesPlayer -= 1;
                if (lifesPlayer == 0) {
                    LoadLostScene();
                }
                Resume();
                ChangeMainCamera();
                objectColission.transform.position = targetNextPosition;
                objectColission = null;
            }
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
                if (miniGameActiveTime > 2)
                {
                    miniGameActiveTime -= 1.0f;
                }
            }
        } else {
            if (other.tag == "Garbage")
            {
                lifesPlayer -= 1;
                if (lifesPlayer == 0) {
                    LoadLostScene();
                } else {
                    ToggleVisibilityForSpecificObjectTwo(indexHearths);
                    ToggleVisibilityForSpecificObject(indexHearths);
                    indexHearths += 1;
                }
            }
        }
    }

    private void Pause()
    {
        // Pausar el movimiento de las plataformas
        FindObjectOfType<SceneMove>().SetPauseState(true);
        // Pausar el movimiento de los objetos generados
        FindObjectOfType<ObjectGenerator>().SetPauseState(true);
        GameIsPaused = true;
        CanvasMiniGame.gameObject.SetActive(true);
    }

    private void Resume()
    {
        // Reanudar el movimiento de las plataformas
        FindObjectOfType<SceneMove>().SetPauseState(false);
        // Reanudar el movimiento de los objetos generados
        FindObjectOfType<ObjectGenerator>().SetPauseState(false);
        GameIsPaused = false;
        CanvasMiniGame.gameObject.SetActive(false);
    }

    void ChangeMiniGameCamera()
    {
        MainCamera.SetActive(!MainCamera.activeSelf);
        MiniGameCamera.SetActive(!MiniGameCamera.activeSelf);
        cameraMiniGameIsActive = true;
        startTime = Time.time;
    }

    void ChangeMainCamera()
    {
        MiniGameCamera.SetActive(false);
        MainCamera.SetActive(true);
        cameraMiniGameIsActive = false;
    }

    bool checkCorrectKeyDown()
    {
        if (objectColission != null)
        {
            Dictionary<string, KeyCode> garbageKeyMap = new Dictionary<string, KeyCode>
            {
                {"BlueGarbage", KeyCode.A},
                {"RedGarbage", KeyCode.F},
                {"GreenGarbage", KeyCode.S},
                {"YellowGarbage", KeyCode.D}
            };
            if (garbageKeyMap.ContainsKey(objectColission.tag) && Input.GetKeyDown(garbageKeyMap[objectColission.tag]))
            {
                score += 10;
                scoreBox.GetComponent<TextMeshProUGUI>().text = "Puntaje: " + score;
                objectColission.transform.position = targetNextPosition;
                return true;
            }
            objectColission.transform.position = targetNextPosition;
        }
        return false;
    }

    void LoadLostScene()
    {
        SceneManager.LoadScene("Lost");
    }

    void ToggleVisibilityForSpecificObject(int index)
    {
        // Verificar si el índice está dentro del rango de la lista
        if (index >= 0 && index < gameObjectsToHide.Count)
        {
            // Invertir la visibilidad del objeto en el índice especificado
            GameObject objToToggle = gameObjectsToHide[index];
            objToToggle.SetActive(!objToToggle.activeSelf);
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango. Asegúrate de que el índice sea válido.");
        }
    }

    void ToggleVisibilityForSpecificObjectTwo(int index)
    {
        // Verificar si el índice está dentro del rango de la lista
        if (index >= 0 && index < gameObjectsToHideTwo.Count)
        {
            // Invertir la visibilidad del objeto en el índice especificado
            GameObject objToToggle = gameObjectsToHideTwo[index];
            objToToggle.SetActive(!objToToggle.activeSelf);
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango. Asegúrate de que el índice sea válido.");
        }
    }
}