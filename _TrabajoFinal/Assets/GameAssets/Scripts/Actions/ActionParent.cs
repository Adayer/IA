using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionParent : MonoBehaviour
{
    [SerializeField] protected CombatManager.ActionType tipoAccion;
    public void Act()
    {
        CombatManager.Instance.Player.ChooseAction(this, tipoAccion);        
    }

    public abstract IEnumerator Effect(TrainerParent trainer);
}
