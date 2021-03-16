using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMarcando : StateCheckParent
{
    [SerializeField] private bool m_marcando = false;

    public override bool Check()
    {
        return m_marcando;
    }
}
