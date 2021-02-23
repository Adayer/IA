using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    [SerializeField] private GameObject m_prefabBullet = null;
    [SerializeField] private Transform m_spawnPoint = null;
 
    void Start()
    {
        
    }

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

        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.GetComponentInParent<RobotsTorreta>() != null)
                {
                    GameObject newBullet = Instantiate(m_prefabBullet, m_spawnPoint.position, Quaternion.identity);
                    newBullet.transform.position = new Vector3(newBullet.transform.position.x, 1f, newBullet.transform.position.z);
                    newBullet.GetComponent<Bala>().CurrentObjective = hit.collider.GetComponentInParent<RobotsTorreta>();
                }                
            }
        }
    }
}
