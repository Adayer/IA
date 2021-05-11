using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPeligroDeGol : StateCheckParent
{
    [SerializeField] private bool m_peligroDeGol = false;

    public override bool Check()
    {
        return m_peligroDeGol;
    }
}
