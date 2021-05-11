using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public enum MODE
    {
        SEEK, SEEK_ARRIVE, FLEE, PURSUE, EVADE, WANDER
    };

    public MODE      m_mode;
    public float     m_mass = 15;
    public float     m_speed = 3;
    public float     m_steeringMax = 15;
    public Transform m_target;

    private Vector3  m_velocity;
    private Vector3  m_targetVelocity;

    // Wander
    private Vector3 m_wanderForce;
    public float    m_circleRadius = 1;
    public float    m_turnChance = 0.05f;
    public float    m_maxRadius = 5;

    private void Start()
    {
        m_velocity       = transform.forward;
        m_targetVelocity = m_target.position;
    }

    // SEEK
    private void SeekBehaviour ()
    {
        SeekBehaviour (m_target.transform.position);
    }

    private void SeekBehaviour (Vector3 targetPosition)
    {
        // Calculamos la velocidad deseada al objetivo
        Vector3 dVelocity = (targetPosition - transform.position).normalized * m_speed;
        
        // Calculamos la fuerza de giro
        Vector3 steering  = Vector3.ClampMagnitude(dVelocity - m_velocity, m_steeringMax) / m_mass;
  
        m_velocity = Vector3.ClampMagnitude(m_velocity + steering, m_speed);
        transform.position += m_velocity * Time.deltaTime;
        transform.forward = m_velocity.normalized;

        Debug.DrawRay(transform.position, m_velocity.normalized * 2, Color.green);
        Debug.DrawRay(transform.position, dVelocity.normalized * 2, Color.magenta);
    }

    // FLEE
    private void FleeBehaviour ()
    {
        FleeBehaviour (m_target.transform.position);
    }

    private void FleeBehaviour (Vector3 targetPosition)
    {
        // Calculamos la velocidad deseada al objetivo
        Vector3 dVelocity = (transform.position - m_target.transform.position).normalized * m_speed;
        
        // Calculamos la fuerza de giro
        Vector3 steering  = Vector3.ClampMagnitude(dVelocity - m_velocity, m_steeringMax) / m_mass;

        m_velocity = Vector3.ClampMagnitude(m_velocity + steering, m_speed);
        transform.position += m_velocity * Time.deltaTime;
        transform.forward = m_velocity.normalized;

        Debug.DrawRay(transform.position, m_velocity.normalized * 2, Color.green);
        Debug.DrawRay(transform.position, dVelocity.normalized * 2, Color.magenta);
    }

    // PURSUE / EVADE
    private void PursueEvadeBehaviour (bool isPursue)
    {
        // Calculamos la distancia al objetivo
        float distance = (m_target.transform.position - transform.position).magnitude;            
        
        //Calculamos nuestra velocidad
        float speed = m_velocity.magnitude;

        //Calculamos el tiempo que tardamos en llegar
        float prediction = distance / speed;           
        
        //Situamos el objetivo donde pensamos que va a situarse.            
        Vector3 explicitTarget = m_target.transform.position + m_targetVelocity * prediction;
        
        if (isPursue)
            SeekBehaviour (explicitTarget);
        else
            FleeBehaviour (explicitTarget);
    }

    // WANDER
    private Vector3 GetWanderForce  ()
    {
        if (Random.value < m_turnChance)
        {
            Vector3 circleCenter = m_velocity.normalized;
            Vector3 randomPoint  = Random.insideUnitCircle;

            Vector3 displacement = new Vector3(randomPoint.x, randomPoint.y) * m_circleRadius;
            displacement = Quaternion.LookRotation(m_velocity) * displacement;

            m_wanderForce = circleCenter + displacement;
        }

        return m_wanderForce;
    }

    private void    WanderBehaviour ()
    {
        var dVelocity = GetWanderForce();
        dVelocity = dVelocity.normalized * m_speed;

        Vector3 steering  = Vector3.ClampMagnitude(dVelocity - m_velocity, m_steeringMax) / m_mass;

        m_velocity = Vector3.ClampMagnitude(m_velocity + steering, m_speed);
        transform.position += m_velocity * Time.deltaTime;
        transform.forward = m_velocity.normalized;

        Debug.DrawRay(transform.position, m_velocity.normalized * 2, Color.green);
        Debug.DrawRay(transform.position, dVelocity.normalized * 2, Color.magenta);
    }

    private void    Update          ()
    {
        // Calculamos velocidad del ojetivo (Necesario para cierto comportamientos)
        m_targetVelocity = m_target.position - m_targetVelocity;

        // La IA se mueve según el modo seleccionado
        switch (m_mode)
        {
            case MODE.SEEK:
                SeekBehaviour ();
                break;
            case MODE.SEEK_ARRIVE:
                SeekBehaviour ();
                break;
            case MODE.FLEE:
                FleeBehaviour ();
                break;
            case MODE.PURSUE:
                PursueEvadeBehaviour (true);
                break;
            case MODE.EVADE:
                PursueEvadeBehaviour (false);
                break;
            case MODE.WANDER:
                WanderBehaviour ();
                break;
            default:
                break;
        }
    }
}
