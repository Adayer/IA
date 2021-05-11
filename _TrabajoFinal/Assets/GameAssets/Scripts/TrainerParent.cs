using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerParent : MonoBehaviour
{
    private PokemonParent m_currentPokemonPicked;    

    private ActionParent m_actionChosen;
    private CombatManager.ActionType m_typeOfActionChosen;

    public CombatManager.ActionType TypeOfActionChosen { get => m_typeOfActionChosen;}
    public ActionParent ActionChosen { get => m_actionChosen; }
    public PokemonParent CurrentPokemonPicked { get => m_currentPokemonPicked; set => m_currentPokemonPicked = value; }

    public void ChooseAction (ActionParent action, CombatManager.ActionType typeOfAction)
    {
        if(action != null)
        {
            m_typeOfActionChosen = typeOfAction;
            m_actionChosen = action;

            CombatManager.Instance.HasPicked();
        }
        //CHANGE UI
    }
}
