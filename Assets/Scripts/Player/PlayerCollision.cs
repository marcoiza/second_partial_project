using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Collision : MonoBehaviour
{
    public Canvas CanvasMiniGame;
    public Canvas CanvasSaveScore;
    public GameObject mainCamera;
    public GameObject miniGameCamera;
    public GameObject scoreBox;
    public Image worldImageTime;
    [SerializeField] TMP_InputField inputUsername;
    [SerializeField] TMP_InputField inputScore;

    private GameObject objectCollision;
    private Vector3 targetCanvasPosition = new Vector3(1.9f, 4.7f, 7f);
    private Vector3 targetNextPosition = new Vector3(0f, 1.3f, -48f);
    private int lifesPlayer = 1;
    private int indexHearts = 0;
    private bool cameraMiniGameIsActive = false;
    private float miniGameActiveTime = 5.0f; // Tiempo en segundos que la cámara MiniGame está activa
    private float startTime;
    private bool isFinishGame = false;
    private bool isSavedScore = false;
    private bool GameIsPaused = false;

    public List<GameObject> gameObjectsToHide = new List<GameObject>();
    public List<GameObject> gameObjectsToHideTwo = new List<GameObject>();

    public static int score { get; private set; } = 0;
    private static string username;

    void Start()
    {
        miniGameCamera.SetActive(false);
        CanvasMiniGame.gameObject.SetActive(false);
        CanvasSaveScore.gameObject.SetActive(false);
        if (scoreBox != null)
        {
            TextMeshProUGUI textMeshPro = scoreBox.GetComponent<TextMeshProUGUI>();

            if (textMeshPro != null)
            {
                textMeshPro.text = "Puntaje: " + score;
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component not found on scoreBox.");
            }
        }
        else
        {
            Debug.LogWarning("scoreBox is null. Check your initialization.");
        }
        miniGameActiveTime = 5.0f;
    }

    void Update()
    {
        if (cameraMiniGameIsActive && !isFinishGame)
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
                        ToggleVisibilityForSpecificObjectTwo(indexHearts);
                        ToggleVisibilityForSpecificObject(indexHearts);
                        indexHearts += 1;
                        lifesPlayer -= 1;
                        if (lifesPlayer == 0) {
                            Pause();
                            ChangeMiniGameCamera();
                            inputScore.text = "" + score;
                            CanvasSaveScore.gameObject.SetActive(true);
                            isFinishGame = true;
                            if (isSavedScore) {
                                LoadLostScene();
                            }
                        }
                    }
                    Resume();
                    ChangeMainCamera();
                    objectCollision = null;
                }
            }
            else 
            {
                indexHearts += 1;
                lifesPlayer -= 1;
                ToggleVisibilityForSpecificObjectTwo(indexHearts);
                ToggleVisibilityForSpecificObject(indexHearts);
                if (lifesPlayer == 0) {
                    Pause();
                    ChangeMiniGameCamera();
                    inputScore.text = "" + score;
                    CanvasSaveScore.gameObject.SetActive(true);
                    if (isSavedScore) {
                        LoadLostScene();
                    }
                    isFinishGame = true;
                }
                Resume();
                ChangeMainCamera();
                objectCollision.transform.position = targetNextPosition;
                objectCollision = null;
            }
        } else {
            if (cameraMiniGameIsActive && isSavedScore) {
                LoadLostScene();
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
                CanvasMiniGame.gameObject.SetActive(true);
                objectCollision = other.gameObject;
                other.transform.position = targetCanvasPosition;
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
                    Pause();
                    ChangeMiniGameCamera();
                    inputScore.text = "" + score;
                    CanvasSaveScore.gameObject.SetActive(true);
                    if (isSavedScore) {
                        LoadLostScene();
                    }
                    isFinishGame = true;
                } else {
                    ToggleVisibilityForSpecificObjectTwo(indexHearts);
                    ToggleVisibilityForSpecificObject(indexHearts);
                    indexHearts += 1;
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
        mainCamera.SetActive(!mainCamera.activeSelf);
        miniGameCamera.SetActive(!miniGameCamera.activeSelf);
        cameraMiniGameIsActive = true;
        startTime = Time.time;
    }

    void ChangeMainCamera()
    {
        miniGameCamera.SetActive(false);
        mainCamera.SetActive(true);
        cameraMiniGameIsActive = false;
    }

    bool checkCorrectKeyDown()
    {
        if (objectCollision != null)
        {
            Dictionary<string, KeyCode> garbageKeyMap = new Dictionary<string, KeyCode>
            {
                {"BlueGarbage", KeyCode.A},
                {"RedGarbage", KeyCode.F},
                {"GreenGarbage", KeyCode.S},
                {"YellowGarbage", KeyCode.D}
            };
            if (garbageKeyMap.ContainsKey(objectCollision.tag) && Input.GetKeyDown(garbageKeyMap[objectCollision.tag]))
            {
                score += 10;
                scoreBox.GetComponent<TextMeshProUGUI>().text = "Puntaje: " + score;
                objectCollision.transform.position = targetNextPosition;
                return true;
            }
            objectCollision.transform.position = targetNextPosition;
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
    public void send()
    {
        username = inputUsername.text;
        StartCoroutine(PostDataToLocalService());
    }

    IEnumerator PostDataToLocalService()
    {
        Debug.Log("Entra a la función");
        string serviceUrl = "http://localhost:8000/users";
        // Asegúrate de que la variable "score" sea un número entero
        string jsonBody = $"{{\"username\":\"{username}\",\"score\":{score}}}";
        
        Debug.Log(jsonBody);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest www = new UnityWebRequest(serviceUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string responseData = www.downloadHandler.text;
            // Procesa la respuesta después del POST según tus necesidades
            Debug.Log(responseData);
        }
        else
        {
            Debug.LogError(www.error);
        }
        isSavedScore = true;
    }
}