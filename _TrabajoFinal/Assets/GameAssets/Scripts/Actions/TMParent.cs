using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMParent : ActionParent
{
    private AppConstants.TipoPokemon m_tipoDeAtaque;
    private AppConstants.TipoDaño m_tipoDeDaño;
    private int m_accuracy;
    private int m_critChance;
    private int m_damage;
    private string m_name;


    public string Name { get => m_name; set => m_name = value; }
    public AppConstants.TipoPokemon TipoDeAtaque { get => m_tipoDeAtaque; set => m_tipoDeAtaque = value; }
    public int CritChanceMultiplier { get => m_critChance; set => m_critChance = value; }
    public int Accuracy { get => m_accuracy; set => m_accuracy = value; }
    public int Damage { get => m_damage; set => m_damage = value; }
    public AppConstants.TipoDaño TipoDeDaño { get => m_tipoDeDaño; set => m_tipoDeDaño = value; }

    public override void Act()
    {
        CombatManager.Instance.Player.ChooseAction(this, tipoAccion);
        Debug.Log("Chose to make an attack");
    }
    public override IEnumerator Effect(TrainerParent attacker)
    {
        print(attacker.CurrentPokemonPicked + " utilizó " + m_name);
        yield return new WaitForSeconds(0.5f);
        float totalDamage = 0;
        if (attacker == CombatManager.Instance.Player)
        {
            CombatManager.Instance.OnDealDamage?.Invoke(this, attacker.CurrentPokemonPicked, CombatManager.Instance.Enemy.CurrentPokemonPicked, ref totalDamage);
        }
        else
        {
            CombatManager.Instance.OnDealDamage?.Invoke(this, attacker.CurrentPokemonPicked, CombatManager.Instance.Player.CurrentPokemonPicked, ref totalDamage);
        }
    }

    public void SetProperties(TMSO data)
    {
        m_tipoDeAtaque = data._tipoDeAtaque;
        m_tipoDeDaño = data._tipoDeDaño;
        m_accuracy = data._accuracy;
        m_damage = data._damage;
        m_name = data._name;
    }

}
