using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionParent : MonoBehaviour
{
    [SerializeField] protected CombatManager.ActionType tipoAccion;
    public abstract void Act();
    

    public abstract IEnumerator Effect(TrainerParent trainer);
}
