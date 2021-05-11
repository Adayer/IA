using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TMParent : ActionParent
{
    private AppConstants.TipoPokemon m_tipoDeAtaque;
    private AppConstants.TipoDaño m_tipoDeDaño;
    private int m_accuracy;
    private int m_damage;

    public TMParent(TMSO data)
    {
        m_tipoDeAtaque = data._tipoDeAtaque;
        m_tipoDeDaño = data._tipoDeDaño;
        m_accuracy = data._accuracy;
        m_damage = data._damage;
    }

    public override IEnumerator Effect()
    {
        Debug.LogError("Ataque no configurado");
        yield return null;
    }

    public bool CheckIfHit()
    {
        return true;
    }
}
