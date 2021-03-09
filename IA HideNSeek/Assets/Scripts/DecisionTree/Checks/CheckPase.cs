using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPase : StateCheckParent
{
    [SerializeField] private bool m_puedoPasar = false;

    public override bool Check()
    {
        return m_puedoPasar;
    }
}
