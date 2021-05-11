using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePokemon : ActionParent
{
    public override IEnumerator Effect()
    {
        Debug.LogError("Cambio pkmn no configurado");
        yield return null;
    }
}
