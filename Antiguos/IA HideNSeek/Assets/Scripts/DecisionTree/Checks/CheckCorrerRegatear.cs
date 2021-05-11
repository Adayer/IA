using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCorrerRegatear : StateCheckParent
{
    [SerializeField] private bool m_esMejorCorrerQueRegatear = false;

    public override bool Check()
    {
        return m_esMejorCorrerQueRegatear;
    }
}