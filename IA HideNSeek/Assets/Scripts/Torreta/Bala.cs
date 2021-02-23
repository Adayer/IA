using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] private float m_speed = 10f;
    [SerializeField] private Transform m_player = null;
    [SerializeField] private float m_maxRotation = 0f;
    [SerializeField] private float m_slowDistance = 0f;


    [SerializeField] private Transform m_tracker = null;

    private Vector3 m_currentDirection = Vector3.zero;
    private bool m_isChasing = true;

    private MeshRenderer m_mesh = null;

    [SerializeField] private RobotsTorreta m_currentObjective = null;

    private Vector3 m_posCurce = Vector3.zero;

    private Vector3 m_vectorObjetivo = Vector3.zero;

    void Start()
    {
        m_currentDirection = m_currentObjective.transform.position - this.transform.position;
        m_mesh = this.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        CalculatePursuit();

        Move();

        Debug.DrawRay(this.transform.position, m_currentDirection, Color.red);
    }

    private void Move()
    {
        //Interplar current direction y direccion destino
        m_currentDirection = Vector3.MoveTowards(m_currentDirection, m_vectorObjetivo, m_maxRotation * Time.deltaTime);

        m_currentDirection.Normalize();

        //Move
        this.transform.position += m_currentDirection * Time.deltaTime * m_speed;
        this.transform.forward = Vector3.Normalize(m_currentDirection);

    }

    private void CalculatePursuit()
    {
        float dist = (m_currentObjective.transform.position - this.transform.position).magnitude;
        float prediction = dist / m_speed;
        
        //Calculo de posición de cruce
        m_posCurce = m_currentObjective.transform.position + m_currentObjective.CurrentSpeed * m_currentObjective.transform.forward * prediction;

        m_posCurce.y = 0f;
        
        //Calculo de direccion objetivo
        m_vectorObjetivo = m_posCurce - this.transform.position;
    }
}
