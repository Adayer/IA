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

    [SerializeField] private Transform m_positionToRespawnIn = null;

    private Transform m_currentObjective = null;
    private Transform m_currentDestination = null;

    private Transform m_bullet;

    private enum States {Patrol, Escaping }

    private States m_currentState = States.Patrol;

    private bool m_avoindingWall = false;
    [SerializeField] private float m_distanceToCheckWalls = 10f;
    [SerializeField] private float m_degreesBetweenWallChecks = 20f;
    [SerializeField] private LayerMask m_wallMask;

    Vector3 vectorToCheck = Vector3.zero;
    Vector3 normalOfVector = Vector3.zero;
    Vector3 obstacleAvoidedPos = Vector3.zero;

    public float Speed { get => m_speed; set => m_speed = value; }

    private void Start()
    {
        m_currentIndex = 0;
        m_currentDirection = m_path.PathPoints[m_currentIndex].position - this.transform.position;
    }

    void Update()
    {
        CheckForObstacle();

        if (!m_avoindingWall)
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
        }
        else
        {
            Vector3 vectorDestino = obstacleAvoidedPos;
            //Interplar current direction y direccion destino
            m_currentDirection = Vector3.MoveTowards(m_currentDirection, vectorToCheck, m_maxRotation * Time.deltaTime);

			Debug.DrawRay(this.transform.position, m_currentDirection, Color.red);

            //Move
            m_currentDirection.Normalize();
            this.transform.position += m_currentDirection * Time.deltaTime * m_speed;

            this.transform.forward = m_currentDirection;

            switch (m_currentState)
            {
                case States.Patrol:
                    {
                        if (!Physics.Raycast(this.transform.position, m_path.PathPoints[m_currentIndex].position - this.transform.position, m_distanceToCheckWalls))
                        {
                            m_avoindingWall = false;
                        }

						Vector3 dir = m_path.PathPoints[m_currentIndex].position - this.transform.position;
						dir.Normalize();
						dir = dir * m_distanceToCheckWalls;

						Debug.DrawLine(this.transform.position, dir + this.transform.position , Color.red);
                    }
                    break;
                case States.Escaping:
                    {
                        if (!Physics.Raycast(this.transform.position, (this.transform.position - m_bullet.position)))
                        {
                            m_avoindingWall = false;
                        }
                    }
                    break;
                default:
                    break;
            }
            
        }
    }
    private void CheckForObstacle()
    {
        RaycastHit hit;
        Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * m_distanceToCheckWalls, Color.green);
        if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, m_distanceToCheckWalls, m_wallMask))
        {
            if(hit.collider.tag == "Obstacle")
            {
                m_avoindingWall = true;

                vectorToCheck = this.transform.forward;

                normalOfVector = new Vector3(hit.normal.z, hit.normal.y, -hit.normal.x);
                normalOfVector.Normalize();

                Vector3 tempVNeg = vectorToCheck - normalOfVector * 0.5f;
                Vector3 tempVPos = vectorToCheck + normalOfVector * 0.5f;

                bool whileCheck = true;
                while(whileCheck)
                {
                    Debug.DrawRay(this.transform.position, tempVPos, Color.blue, 5f);
                    Debug.DrawRay(this.transform.position, tempVNeg, Color.blue, 5f);

                    if (!Physics.Raycast(this.transform.position, tempVNeg, out hit, m_distanceToCheckWalls))
                    {
                        whileCheck = false;
                        vectorToCheck = tempVNeg;
                        break;
                    }

                    if (!Physics.Raycast(this.transform.position, tempVPos, out hit, m_distanceToCheckWalls))
                    {
                        whileCheck = false;
                        vectorToCheck = tempVPos;
                        break;
                    }

                    if (whileCheck)
                    {
                        tempVNeg = tempVNeg - normalOfVector * 0.25f;
                        tempVPos = tempVPos + normalOfVector * 0.25f;
                    }
                }
                vectorToCheck.Normalize();
                obstacleAvoidedPos = vectorToCheck;
                
            }
        }
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
        Debug.DrawRay(this.transform.position, vectorDestino * 10f, Color.blue);

        //Interplar current direction y direccion destino
        m_currentDirection = Vector3.MoveTowards(m_currentDirection, vectorDestino, m_maxRotation * Time.deltaTime);
        //Move
        m_currentDirection.Normalize();
        this.transform.position += m_currentDirection * Time.deltaTime * m_speed;

        this.transform.forward = m_currentDirection;
    }

    private void Escaping()
    {
        //Calculo de destino
        Vector3 vectorDestino = Vector3.Normalize(this.transform.position - m_bullet.position);

        Debug.DrawRay(this.transform.position, vectorDestino * 10f, Color.blue);
        //Interplar current direction y direccion destino
        m_currentDirection = Vector3.MoveTowards(m_currentDirection, vectorDestino, m_maxRotation * Time.deltaTime);
        //Move
        m_currentDirection.Normalize();
        this.transform.position += m_currentDirection * Time.deltaTime * m_speed;

        this.transform.forward = m_currentDirection;
    }

    public void DestroyAndSpawn()
    {
        this.transform.position = m_positionToRespawnIn.position;           
        m_currentIndex = 0;        
        m_currentState = States.Patrol;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Bala>() != null && m_currentState == States.Patrol)
        {
            m_bullet = other.transform;
            m_currentState = States.Escaping;
        }
    }

}
