using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOfrecermeAlPase : StateCheckParent
{
    [SerializeField] private bool m_puedoOfrecermeAlPase = false;

    public override bool Check()
    {
        return m_puedoOfrecermeAlPase;
    }
}
