using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public List<GameObject> objects;
    public float generationTime;
    private float timer;
    public float initialSpeed;
    public float speedIncrease;
    private int objetosGenerados = 0; // Contador de objetos generados

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= generationTime)
        {
            timer = 0f;
            GenerateObject();
        }
    }

    public void GenerateObject()
    {
        int prefabIndex = Random.Range(0, objects.Count);
        Vector3 position = GenerateRandomPosition();
        // Instantiate(objects[prefabIndex], position, Quaternion.identity);
        GameObject objectAux = Instantiate(objects[prefabIndex], position, Quaternion.identity);
        StartCoroutine(MoveObjectIndependent(objectAux));
        objetosGenerados++; // Incrementar el contador de objetos generados

        if (objetosGenerados >= 3 && generationTime>=1 && speedIncrease < 1f) // Comprobar si se han generado 10 objetos
        {
            generationTime -= 0.5f; // Aumentar el tiempo de generación
            speedIncrease += 0.005f;
        }
    }

    Vector3 GenerateRandomPosition()
    {
        int index = Random.Range(0, 3);

        // Selecciona el valor según el índice
        float value = 0;
        switch (index)
        {
            case 0:
                value = 1.5f;
                break;
            case 1:
                value = 0.0f;
                break;
            case 2:
                value = -1.5f;
                break;
        }
        float x = value;
        //float y = transform.position.y;
        //float z = transform.position.z;
        return new Vector3(x, 1.2f, 0);
    }

    IEnumerator MoveObjectIndependent(GameObject objectAux)
    {
        float currentSpeed = initialSpeed; // Start with initial speed
        while (true)
        {
            objectAux.transform.Translate(new Vector3(0, 0, -currentSpeed) * Time.deltaTime);
            currentSpeed += speedIncrease; // Increase speed each frame
            yield return null;
        }
    }
}
