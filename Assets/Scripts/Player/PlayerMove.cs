using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    void Update()
    {
        //Si pulsa la flecha a la izquierda se mueve el personaje una unidad a la izquierda
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (this.gameObject.transform.position.x > LevelBoundary.leftSide) 
            { 
                transform.Translate(new Vector3(-1.5f,0,0) );
            }
        }
        //Si pulsa la flecha a la derecha se mueve el personaje una unidad a la derecha
        if (Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            if (this.gameObject.transform.position.x < LevelBoundary.rightSide)
            {
                transform.Translate(new Vector3(1.5f,0,0) );
            }
        }
    }
}
