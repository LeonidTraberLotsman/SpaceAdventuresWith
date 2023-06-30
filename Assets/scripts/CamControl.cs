using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    bool canMove = true;
    float speed = 0.01f;
    int XMax = 30;
    int ZMax = 30;

    int YMax = 20;
    int YMin = 4;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.W)&&transform.position.z<XMax)
            {
                transform.position+= new Vector3(0,0,1)*speed*transform.position.y;
            }
            if (Input.GetKey(KeyCode.S) && transform.position.z >0)
            {
                transform.position += new Vector3(0, 0, -1) * speed * transform.position.y;
            }

            if (Input.GetKey(KeyCode.D) && transform.position.x < ZMax)
            {
                transform.position += new Vector3(1, 0, 0) * speed * transform.position.y;
            }
            if (Input.GetKey(KeyCode.A) && transform.position.x > 0)
            {
                transform.position += new Vector3(-1, 0, 0) * speed * transform.position.y;
            }

            if (Input.GetKey(KeyCode.Q) && transform.position.y < YMax)
            {
                transform.position += new Vector3(0, 1, 0) * speed * transform.position.y;
            }
            if (Input.GetKey(KeyCode.E) && transform.position.y > YMin)
            {
                transform.position += new Vector3(0,-1, 0) * speed * transform.position.y;
            }
        }
    }
}
