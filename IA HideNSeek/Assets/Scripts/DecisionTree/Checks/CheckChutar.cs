using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckChutar : StateCheckParent
{
    [SerializeField] private bool m_puedoChutar = false;

    public override bool Check()
    {
        return m_puedoChutar;
    }
}

