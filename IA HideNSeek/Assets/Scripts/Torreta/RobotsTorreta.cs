using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsTorreta : MonoBehaviour
{
    [SerializeField] private float m_speed = 10f;
    [SerializeField] private float m_maxRotation = 0f;
    [SerializeField] private float m_slowDistance = 0f;
       
    private Vector3 m_currentDirection = Vector3.zero;

    private bool m_isEscaping = true;

    [SerializeField] private PathLocations m_path = null;
    private int m_currentIndex = 0;


    [SerializeField] private Transform m_currentObjective = null;
    [SerializeField] private Transform m_currentDestination = null;

    private Transform m_bullet;

    private enum States {Patrol, Escaping }

    private States m_currentState = States.Patrol;

    public float Speed { get => m_speed; set => m_speed = value; }

    private void Start()
    {
        m_currentIndex = 0;
        m_currentDirection = m_path.PathPoints[m_currentIndex].position - this.transform.position;
    }

    void Update()
    {

        switch (m_currentState)
        {
            case States.Patrol:
                {
                    Patrolling();
                }
                break;
            case States.Escaping:
                {
                    Escaping();
                }
                break;
            default:
                break;
        }

        Patrolling();

        Debug.DrawRay(this.transform.position, m_currentDirection, Color.red, 1f);

    }

    private void Patrolling()
    {
        float distance = Vector3.Distance(this.transform.position, m_path.PathPoints[m_currentIndex].position);

        if (distance <= m_slowDistance)
        {
            if (m_currentIndex == (m_path.PathPoints.Count - 1))
            {
                m_currentIndex = 0;
            }
            else
            {
                m_currentIndex++;
            }
        }

        //Calculo de destino
        Vector3 vectorDestino = Vector3.Normalize(m_path.PathPoints[m_currentIndex].position - this.transform.position);

        //Interplar current direction y direccion destino
        m_currentDirection = Vector3.MoveTowards(m_currentDirection, vectorDestino, m_maxRotation * Time.deltaTime);
        //Move
        m_currentDirection.Normalize();
        this.transform.position += m_currentDirection * Time.deltaTime * m_speed;

        this.transform.forward = Vector3.Normalize(m_currentDirection);
    }

    private void Escaping()
    {
        //Calculo de destino
        Vector3 vectorDestino = Vector3.Normalize(m_bullet.position - this.transform.position);
        vectorDestino -= vectorDestino;

        //Interplar current direction y direccion destino
        m_currentDirection = Vector3.MoveTowards(m_currentDirection, vectorDestino, m_maxRotation * Time.deltaTime);
        //Move
        m_currentDirection.Normalize();
        this.transform.position += m_currentDirection * Time.deltaTime * m_speed;

        this.transform.forward = Vector3.Normalize(m_currentDirection);

    }

    public void DestroyAndSpawn()
    {
        this.transform.position += new Vector3(Random.Range(-50, 50), 0 , Random.Range(-50, 50));      
        m_currentState = States.Patrol;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Bala>() != null)
        {
            m_bullet = other.transform;
            m_currentState = States.Escaping;
        }
    }

}
