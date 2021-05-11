using UnityEngine;
using System.Collections;
using System;
public class Node : IComparable
{
    public float   m_nodeTotalCost;
    public float   m_estimatedCost;
    public bool    m_bObstacle;
    public Node    m_parent;
    public Vector3 m_position;

    public Node()
    {
        m_estimatedCost = 0.0f;
        m_nodeTotalCost = 1.0f;
        m_bObstacle     = false;
        m_parent        = null;
    }

    public Node(Vector3 pos)
    {
        m_estimatedCost = 0.0f;
        m_nodeTotalCost = 1.0f;
        m_bObstacle     = false;
        m_parent        = null;
        m_position      = pos;
    }

    public void MarkAsObstacle()
    {
        m_bObstacle = true;
    }

    public int CompareTo(object obj)
    {
         Node node = (Node)obj;
         //Negative value means object comes before this in the sort order.
         if (m_estimatedCost < node.m_estimatedCost)
            return -1;
         //Positive value means object comes after this in the sort order.
         if (m_estimatedCost > node.m_estimatedCost) return 1;
            return 0;
     }
}

