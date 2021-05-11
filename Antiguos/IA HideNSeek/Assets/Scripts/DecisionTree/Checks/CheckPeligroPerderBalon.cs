using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPeligroPerderBalon : StateCheckParent
{
    [SerializeField] private bool m_peligroPerderBalon = false;

    public override bool Check()
    {
        return m_peligroPerderBalon;
    }
}
