using UnityEngine;
using System.Collections;

public class PriorityQueue
{
    private ArrayList m_nodes = new ArrayList();

    public int Length
    {
        get
        {
            return m_nodes.Count;
        }
    }

    public bool Contains(object node)
    {
        return m_nodes.Contains(node);
    }

    public Node First()
    {
        if (m_nodes.Count > 0)
        {
            return (Node)m_nodes[0];
        }
        return null;
    }

    public void Push(Node node)
    {
        m_nodes.Add(node);
        m_nodes.Sort();
    }

    public void Remove(Node node)
    {
        m_nodes.Remove(node);
        //Ensure the list is sorted
        m_nodes.Sort();
    }
}
