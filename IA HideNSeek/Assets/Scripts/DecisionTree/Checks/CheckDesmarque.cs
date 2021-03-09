using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDesmarque : StateCheckParent
{
    [SerializeField] private bool m_puedoDesmarcarme = false;

    public override bool Check()
    {
        return m_puedoDesmarcarme;
    }
}