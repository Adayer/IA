using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericQueue<T> where T : class
{
    public Node head; //Primer nodo de la fila
    private Node tail; //Último nodo de la fila
    public int count; //Número de nodos totales

    //Los nodos tienen la información del tipo genérico además del nodo anterior y posterior
    public class Node
    {
        public T data;
        public Node previous;
        public Node next;

        public Node(T data)
        {
            //Debug.LogError(data);
            this.data = data;
        }

    }

    //Añadir un nuevo nodo a la fila
    public void PonerALaFila(T nodeType)
    {
        //Debug.LogError(nodeType);
        Node newNode = new Node(nodeType);
        newNode.previous = tail;

        if (count == 0)
        {
            tail = newNode;
            head = newNode;
            //Debug.LogError(head.data);
        }

        if (tail != null)
        {
            tail.next = newNode;
            tail = newNode;
        }

        count++;
    }

    //Quitar el primer nodo de la fila y devolver el valor
    public T QuitarDeLaFila()
    {
        T returnedData = head.data;
        head = head.next;
        count--;
        //Debug.LogError("returned" + returnedData);
        return returnedData;
    }
}
