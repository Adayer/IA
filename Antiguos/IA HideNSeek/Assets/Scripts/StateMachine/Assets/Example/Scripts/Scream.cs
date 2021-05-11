using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scream : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string m_cuote;

    void Start()
    {
        print(m_cuote);
        animator.SetTrigger("Idle");
    }
}
