using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar
{
    public static PriorityQueue openList;
    public static HashSet<Node> closedList;

    private static float HeuristicEstimateCost(Node curNode, Node goalNode)
    {
        Vector3 vecCost = curNode.m_position - goalNode.m_position;
        return vecCost.magnitude;
    }

    public static ArrayList FindPath(Node start, Node goal)
    {
        openList = new PriorityQueue();
        openList.Push(start);
        start.m_nodeTotalCost = 0.0f;
        start.m_estimatedCost = HeuristicEstimateCost(start, goal);
        closedList = new HashSet<Node>();
        Node node = null;

        while (openList.Length != 0)
        {
            node = openList.First();
            //Check if the current node is the target node
            if (node.m_position == goal.m_position)
            {
                return CalculatePath(node);
            }

            //Create an ArrayList to store the neighboring nodes
            ArrayList neighbours = new ArrayList();
            GridManager.instance.GetNeighbours(node, neighbours);
            for (int i = 0; i < neighbours.Count; i++)
            {
                Node neighbourNode = (Node)neighbours[i];
                if (!closedList.Contains(neighbourNode))
                {
                    float cost = HeuristicEstimateCost(node, neighbourNode);
                    float totalCost = node.m_nodeTotalCost + cost;
                    float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);
                    neighbourNode.m_nodeTotalCost = totalCost;
                    neighbourNode.m_parent = node;
                    neighbourNode.m_estimatedCost = totalCost +
                    neighbourNodeEstCost;

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Push(neighbourNode);
                    }
                }
            }

            //Add the current node to the closed list
            closedList.Add(node);

            //and remove it from openList
            openList.Remove(node);
        }

        if (node.m_position != goal.m_position)
        {
            Debug.LogError("Goal Not Found");
            return null;
        }

        return CalculatePath(node);
    }

    private static ArrayList CalculatePath(Node node)
    {
        ArrayList list = new ArrayList();
        while (node != null)
        {
            list.Add(node);
            node = node.m_parent;
        }
        list.Reverse();
        return list;
    }
}
