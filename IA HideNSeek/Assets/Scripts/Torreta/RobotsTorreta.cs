using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsTorreta : MonoBehaviour
{
    [SerializeField] private float m_speed = 10f;
    [SerializeField] private Transform m_player = null;
    [SerializeField] private float m_maxRotation = 0f;
    [SerializeField] private float m_slowDistance = 0f;

    [SerializeField] private Material m_isChasingMat = null;
    [SerializeField] private Material m_isBeingChasedMat = null;


    private Vector3 m_currentDirection = Vector3.zero;
    private bool m_isChasing = true;

    private float m_currentSpeed = 0f;
    
    private MeshRenderer m_mesh = null;

    public float CurrentSpeed { get => m_currentSpeed; }

    void Start()
    {
        m_mesh = this.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        //Calculo de destino
        Vector3 vectorDestino = Vector3.Normalize(m_player.position - this.transform.position);

        //Si esta escapand el destino se niega
        if (!m_isChasing)
        {
            vectorDestino = -vectorDestino;
        }

        //Interplar current direction y direccion destino
        m_currentDirection = Vector3.MoveTowards(m_currentDirection, vectorDestino, m_maxRotation * Time.deltaTime);


        //Slow when near
        float speedMod = Vector3.Distance(this.transform.position, m_player.position) / m_slowDistance;
        m_currentSpeed = m_speed;

        if (speedMod < 1f && m_isChasing)
        {
            m_currentSpeed *= speedMod;
        }


        //Move
        this.transform.position += m_currentDirection * Time.deltaTime * m_currentSpeed;
        //this.transform.forward = Vector3.Normalize(m_currentDirection);
        this.transform.LookAt((this.transform.position + m_currentDirection));

        Debug.DrawRay(this.transform.position, m_currentDirection, Color.red, 1f);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            PillarEscapar();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == m_player)
        {
            PillarEscapar();
        }
    }

    private void PillarEscapar()
    {
        m_isChasing = !m_isChasing;
        print(m_isChasing);
        if (m_isChasing)
        {
            m_mesh.sharedMaterial = m_isChasingMat;
        }
        else
        {
            m_mesh.sharedMaterial = m_isBeingChasedMat;
        }
    }

}
