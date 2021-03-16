using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAyuda : StateCheckParent
{
    [SerializeField] private bool m_puedoAyudar = false;

    public override bool Check()
    {
        return m_puedoAyudar;
    }
}
