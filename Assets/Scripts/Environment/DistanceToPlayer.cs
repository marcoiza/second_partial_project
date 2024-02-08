using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToPlayer : MonoBehaviour
{
    public GameObject player; // Objeto del personaje principal
    public float minDistance = 10f; // Distancia mínima al personaje

    void Update()
    {
        // Calcular la distancia al personaje
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Si la distancia es menor que la distancia mínima, mover el prefab
        if (distance < minDistance)
        {
            transform.position = GenerateRandomPosition();
        }
    }

    Vector3 GenerateRandomPosition()
    {
        // Genera una posición aleatoria dentro de la zona de generación
        Vector3 min = transform.position - transform.localScale / 2;
        Vector3 max = transform.position + transform.localScale / 2;
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }
}
