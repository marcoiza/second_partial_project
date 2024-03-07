using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadData : MonoBehaviour
{
    public GameObject objectContainer;
    public GameObject buttonGroupObject;
    public Button usernameButtonPrefab;
    public Button scoreButtonPrefab;
    private string serviceUrl = "http://localhost:8000/users/list";
    void Start()
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(serviceUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string responseData = www.downloadHandler.text;
            // Process the response as needed
            Debug.Log(responseData); // For testing purposes

            List<UserData> userList = ParseJsonManually(responseData);

            foreach (UserData userData in userList)
            {
                // Obtén los datos del usuario
                string username = userData.username;
                int score = userData.score;

                // Instancia botones y configura sus valores
                // StartCoroutine(InstantiateButtonCoroutine(usernameButtonPrefab, username));
                // StartCoroutine(InstantiateButtonCoroutine(scoreButtonPrefab, score.ToString()));
                InstantiateAndConfigureButtons(username, score);
            }
        }
        else
        {
            Debug.LogError(www.error);
        }
    }

    List<UserData> ParseJsonManually(string json)
    {
        List<UserData> userList = new List<UserData>();

        // Suponiendo que tu JSON tiene una estructura como: [{"username":"user1","score":100},{"username":"user2","score":200},...]
        // Accede manualmente a los datos
        string[] userEntries = json.Replace("[", "").Replace("]", "").Split('}');

        foreach (string entry in userEntries)
        {
            if (!string.IsNullOrEmpty(entry))
            {
                string cleanedEntry = entry.Replace("{", "").Replace("}", "");
                string[] keyValuePairs = cleanedEntry.Split(',');

                UserData userData = new UserData();

                foreach (string pair in keyValuePairs)
                {
                    string[] keyValue = pair.Split(':');

                    if (keyValue.Length == 2)  // Asegura que haya dos elementos (clave y valor)
                    {
                        string key = keyValue[0].Replace("\"", "").Trim();
                        string value = keyValue[1].Replace("\"", "").Trim();

                        if (key.Equals("username"))
                            userData.username = value;
                        else if (key.Equals("score"))
                        {
                            // Asegúrate de manejar el caso en que "value" no sea un número válido
                            int parsedScore;
                            if (int.TryParse(value, out parsedScore))
                                userData.score = parsedScore;
                            else
                                Debug.LogError("Error al convertir score a entero.");
                        }
                    }
                }

                userList.Add(userData);
            }
        }

        return userList;
    }

    void InstantiateAndConfigureButtons(string username, int score)
    {
        GameObject buttonGroupInstance = Instantiate(buttonGroupObject, objectContainer.transform);
    
        // Obtener las referencias a los botones directamente desde buttonGroupInstance
        Button usernameButtonInstance = buttonGroupInstance.transform.Find("Username").GetComponent<Button>();
        Button scoreButtonInstance = buttonGroupInstance.transform.Find("Score").GetComponent<Button>();
    
        // Configurar el texto de los botones
        TextMeshProUGUI usernameButtonTextComponent = usernameButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI scoreButtonTextComponent = scoreButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
    
        if (usernameButtonTextComponent != null && scoreButtonTextComponent != null)
        {
            usernameButtonTextComponent.text = username;
            scoreButtonTextComponent.text = score.ToString();
        }
        else
        {
            Debug.LogError("Error: No se encontró el componente de texto en uno o ambos botones.");
        }
    
        // Añadir la instancia de buttonGroupObject al objectContainer
        buttonGroupInstance.transform.SetParent(objectContainer.transform);
    }
}

[System.Serializable]
public class UserData
{
    public string username;
    public int score;
}