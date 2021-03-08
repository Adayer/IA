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

    [SerializeField] private float m_slowDistance = 0f;

    [SerializeField] private Material m_isChasingMat = null;
    [SerializeField] private Material m_isBeingChasedMat = null;


    private MeshRenderer m_mesh = null;
    
    // Start is called before the first frame update
    void Start()
    {
        m_currentDirection = Vector3.Normalize(m_player.position - this.transform.position);
        this.transform.forward = m_currentDirection;
        m_mesh = this.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorDestino = Vector3.Normalize(m_player.position - this.transform.position);

        if (!m_isChasing)
        {
            vectorDestino = -vectorDestino;
        } 

        m_currentDirection = Vector3.MoveTowards(m_currentDirection, vectorDestino, m_maxRotation * Time.deltaTime);

        float speedMod = Vector3.Distance(this.transform.position, m_player.position) / m_slowDistance;

        float currentSpeed = m_speed;

        if(speedMod < 1f && m_isChasing)
        {
            currentSpeed *= speedMod;
        }

        this.transform.position += m_currentDirection * Time.deltaTime * currentSpeed;
        this.transform.forward = m_currentDirection;

		Debug.DrawLine(this.transform.position, this.transform.position + m_currentDirection * 10f , Color.red);
		Debug.DrawLine(this.transform.position, this.transform.position + vectorDestino * 10f , Color.blue);
		


		if (Input.GetKeyDown(KeyCode.Space))
        {
            PillarEscapar();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == m_player)
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
