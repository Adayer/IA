using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttDef : StateCheckParent
{
    [SerializeField] private bool m_ataco = false;

    public override bool Check()
    {
        return m_ataco;
    }
}
