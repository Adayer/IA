using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionNodePosition : DecisionActionParent
{
    public DecisionActionParent[] m_states = new DecisionActionParent[3];

    public enum Position {Defensa, Centro, Delantero  }

    public Position m_currentPosition = Position.Defensa; 

    public override void Execute()
    {
        switch (m_currentPosition)
        {
            case Position.Defensa:
                {
                    m_states[0].Execute();
                }
                break;
            case Position.Centro:
                {
                    m_states[1].Execute();

                }
                break;
            case Position.Delantero:
                {
                    m_states[2].Execute();
                }
                break;
            default:
                break;
        }
    }
}
