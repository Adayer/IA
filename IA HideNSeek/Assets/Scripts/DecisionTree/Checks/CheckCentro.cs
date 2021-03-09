using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCentro : StateCheckParent
{
    [SerializeField] private bool m_puedoCentrar = false;

    public override bool Check()
    {
        return m_puedoCentrar;
    }
}
