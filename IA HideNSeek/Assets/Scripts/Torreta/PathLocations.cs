using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLocations : MonoBehaviour
{
    [SerializeField] private List<Transform> m_pathPoints = new List<Transform>(0);

    public List<Transform> PathPoints { get => m_pathPoints; set => m_pathPoints = value; }

    void OnDrawGizmos()
    {
        for (int i = 0; i < m_pathPoints.Count; i++)
        {
            if (i + 1 < m_pathPoints.Count)
            {
                Debug.DrawLine(m_pathPoints[i].position, m_pathPoints[i + 1].position, Color.red);
            }
        }
    }
}
