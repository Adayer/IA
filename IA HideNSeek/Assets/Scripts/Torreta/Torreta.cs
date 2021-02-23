using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        Vector3 mousePos = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            mousePos = hit.point;
            mousePos.y = this.transform.position.y;
        }

        this.transform.LookAt(mousePos);
    }
}
