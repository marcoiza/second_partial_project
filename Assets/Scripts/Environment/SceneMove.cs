using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMove : MonoBehaviour
{
    private ObjectGenerator objectGenerator;
    private float speed = -5f;

    void Update()
    {
        transform.position += new Vector3(0, 0, speed) * Time.deltaTime;

        if (objectGenerator != null)
        {
            objectGenerator.GenerateObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destroy"))
        {
            Destroy(gameObject);
        }
    }
}
