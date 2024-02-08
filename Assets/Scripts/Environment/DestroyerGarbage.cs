using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerGarbage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("2Destroy"))
        {
            Destroy(gameObject);
        }
    }
}
