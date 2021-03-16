using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFalta : StateCheckParent
{
    [SerializeField] private bool m_faltaNecesaria = false;

    public override bool Check()
    {
        return m_faltaNecesaria;
    }
}
