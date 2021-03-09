using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionNode : DecisionActionParent
{
    public DecisionActionParent m_true;
    public DecisionActionParent m_false;

    private StateCheckParent m_check;

    private void Awake()
    {
        m_check = this.GetComponent<StateCheckParent>();
    }

    public override void Execute ()
    {
        if (m_check.Check())
        {
            m_true.Execute();
        }
        else
        {
            m_false.Execute();
        }
    }
}
