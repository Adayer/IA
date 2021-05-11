using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            this.transform.position = this.transform.position + new Vector3(0f, 0f, -10f);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.transform.position = this.transform.position + new Vector3(-10f, 0f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.transform.position = this.transform.position + new Vector3(0f, 0f, 10f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            this.transform.position = this.transform.position + new Vector3(10f, 0f, 0f);
        }
    }
}
