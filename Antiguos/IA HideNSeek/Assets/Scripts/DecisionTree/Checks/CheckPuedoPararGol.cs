using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPuedoPararGol : StateCheckParent
{
    [SerializeField] private bool m_puedoPararlo = false;

    public override bool Check()
    {
        return m_puedoPararlo;
    }
}
