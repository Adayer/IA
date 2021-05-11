using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillaPillaBehaviour : MonoBehaviour
{
    [SerializeField] private float m_speed = 10f;

    private bool m_isChasing = true;

    [SerializeField] private Transform m_player = null;

    private Vector3 m_currentDirection = Vector3.zero;

    [SerializeField] private float m_maxRotation = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        m_currentDirection = Vector3.Normalize(m_player.position - this.transform.position) * m_speed;       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorDestino = Vector3.Normalize(m_player.position - this.transform.position) * m_speed;

        if (!m_isChasing)
        {
            vectorDestino = -vectorDestino;
        }
        Vector3 vectorFuerza = vectorDestino - m_currentDirection;

        vectorFuerza = Vector3.Normalize(vectorFuerza) * m_maxRotation;
        m_currentDirection = Vector3.Normalize(m_currentDirection + vectorFuerza) * m_speed;

        this.transform.Translate(m_currentDirection * Time.deltaTime);
        Debug.DrawRay(this.transform.position, m_currentDirection, Color.red, 1f);

        this.transform.forward = Vector3.Normalize(m_currentDirection);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_isChasing = !m_isChasing;
       
        }
    }
}
