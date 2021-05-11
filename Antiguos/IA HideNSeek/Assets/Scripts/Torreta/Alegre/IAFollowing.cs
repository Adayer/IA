using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAFollowing : MonoBehaviour
{
    public Path  m_path;
    public float m_speed = 20.0f;
    public float m_mass  = 5.0f;

    //Actual speed of the vehicle
    private float curSpeed;
    private int curPathIndex;
    private float pathLength;
    private Vector3 targetPoint;
    public float m_steeringMax = 15;
    Vector3 velocity;

    void Start ()
    {
        pathLength = m_path.Length;
        curPathIndex = 0;
        velocity = transform.forward;
    }

    void Update () {

        curSpeed = m_speed * Time.deltaTime;
        targetPoint = m_path.GetPoint(curPathIndex);

        if (Vector3.Distance(transform.position, targetPoint) < m_path.Radius)
        {
            if (curPathIndex < pathLength - 1)
                curPathIndex++;
            else 
                curPathIndex = 0;
        }

        if (curPathIndex >= pathLength )
            return;

        // Calculamos la velocidad deseada al objetivo
        Vector3 dVelocity = (targetPoint - transform.position).normalized * curSpeed;
        
        // Calculamos la fuerza de giro
        Vector3 steering  = Vector3.ClampMagnitude(dVelocity - velocity, m_steeringMax) / m_mass;

        velocity = Vector3.ClampMagnitude(velocity + steering, m_speed);
        transform.position += velocity;
        transform.forward = velocity.normalized;
        
        Debug.DrawRay(transform.position, velocity.normalized * 2, Color.green);
        Debug.DrawRay(transform.position, dVelocity.normalized * 2, Color.magenta);
    }
}


