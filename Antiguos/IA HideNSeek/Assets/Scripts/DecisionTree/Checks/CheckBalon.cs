using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBalon : StateCheckParent
{
    [SerializeField] private bool m_tengoBalon = false;

    public override bool Check()
    {
        return m_tengoBalon;
    }
}

