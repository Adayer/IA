using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] private float m_speed = 10f;
    [SerializeField] private float m_maxRotation = 0f;
    [SerializeField] private float m_slowDistance = 0f;


    private Vector3 m_currentDirection = Vector3.zero;
    private bool m_isChasing = true;
	

    private RobotsTorreta m_currentObjective = null;

    private Vector3 m_posCurce = Vector3.zero;

    private Vector3 m_vectorObjetivo = Vector3.zero;

    public RobotsTorreta CurrentObjective { get => m_currentObjective; set => m_currentObjective = value; }

    void Start()
    {
        m_currentDirection = m_currentObjective.transform.position - this.transform.position;
    }

    void Update()
    {
        CalculatePursuit();

        Move();

        Debug.DrawRay(this.transform.position, m_currentDirection, Color.red);
        Debug.DrawRay(this.transform.position, m_vectorObjetivo, Color.blue);
    }

    private void Move()
    {
        //Interplar current direction y direccion destino
        m_currentDirection = Vector3.MoveTowards(m_currentDirection, m_vectorObjetivo, m_maxRotation * Time.deltaTime);

        m_currentDirection.Normalize();

        //Move
        this.transform.position += m_currentDirection * Time.deltaTime * m_speed;
        this.transform.forward = m_currentDirection;

    }

    private void CalculatePursuit()
    {
        float dist = (m_currentObjective.transform.position - this.transform.position).magnitude;
        float prediction = dist / m_speed;
        
        //Calculo de posición de cruce
        m_posCurce = m_currentObjective.transform.position + m_currentObjective.Speed * m_currentObjective.transform.forward * prediction;

        //Calculo de direccion objetivo
        m_vectorObjetivo = m_posCurce - this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<RobotsTorreta>() == m_currentObjective)
        {
            m_currentObjective.DestroyAndSpawn();
            Destroy(this.gameObject);
        }
    }

}
