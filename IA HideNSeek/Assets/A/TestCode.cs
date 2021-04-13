using UnityEngine;
using System.Collections;
public class TestCode : MonoBehaviour
{
    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }
    public ArrayList pathArray;
    GameObject objStartCube, objEndCube;
    private float elapsedTime = 0.0f;
    //Interval time between pathfinding
    public float intervalTime = 1.0f;

    Node currentNode = null;
    bool moviendose = true;

    void Start ()
    {
        objStartCube = GameObject.FindGameObjectWithTag("Start");
        objEndCube = GameObject.FindGameObjectWithTag("End");
        pathArray = new ArrayList();
        FindPath();
        currentNode = (Node)pathArray[0];
        pathArray.RemoveAt(0);
        moviendose = true;
    }

    void Update ()
    {
        if (moviendose)
        {
            print(this.transform.position + "-------------" + currentNode.m_position);


            this.transform.position = Vector3.MoveTowards(this.transform.position, currentNode.m_position, 10 * Time.deltaTime);

            if (Vector3.Distance(this.transform.position, currentNode.m_position) == 0)
            {
                if (pathArray.Count != 0)
                {
                    FindPath();
                    pathArray.RemoveAt(0);
                    currentNode = (Node)pathArray[0];
                }
                else
                {
                    print("Has llegado");
                    moviendose = false;
                }
            }
        }
        elapsedTime += Time.deltaTime;

        //if (elapsedTime >= intervalTime)
        //{
        //    if (moviendose)
        //    {
        //        print(this.transform.position + "-------------" + currentNode.m_position);

        //        this.transform.position = currentNode.m_position;

        //            if (pathArray.Count != 0)
        //            {
        //                FindPath();
        //                pathArray.RemoveAt(0);
        //                currentNode = (Node)pathArray[0];
        //            }
        //            else
        //            {
        //                print("Has llegado");
        //                moviendose = false;
        //            }
        //        elapsedTime = 0.0f;
        //    }
    }

    void FindPath()
    {
        startPos = objStartCube.transform;
        endPos = objEndCube.transform;
        startNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
        goalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));
        pathArray = AStar.FindPath(startNode, goalNode);
    }

    void OnDrawGizmos()
    {
        if (pathArray == null)
            return;

        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];
                    Debug.DrawLine(node.m_position, nextNode.m_position,
                    Color.green);
                    index++;
                }
            }
        }
    }
}