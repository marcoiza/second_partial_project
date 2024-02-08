using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5;
    public float sideSpeed = 3;
    private Animator anim;
    public float x, y;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            if (this.gameObject.transform.position.x > LevelBoundary.leftSide) 
            { 
                transform.Translate(new Vector3(-1.5f,0,0) );
                // anim.SetFloat("VelY", 0);
                // anim.SetFloat("VelX", -1.5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            if (this.gameObject.transform.position.x < LevelBoundary.rightSide)
            {
                transform.Translate(new Vector3(1.5f,0,0) );
                // anim.SetFloat("VelY", 0);
                // anim.SetFloat("VelX", 1.5f);
            }
        }
    }
}
