using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muerte : MonoBehaviour
{
    [SerializeField] private string m_cuote;

    void Start()
    {
        print(m_cuote);
        Destroy(this.gameObject);
    }
}
