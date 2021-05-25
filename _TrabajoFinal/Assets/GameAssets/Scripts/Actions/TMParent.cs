using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMParent : ActionParent
{
    private AppConstants.TipoPokemon m_tipoDeAtaque;
    private AppConstants.TipoDaño m_tipoDeDaño;
    private int m_accuracy;
    private int m_damage;
    private string m_name;

    public string Name { get => m_name; set => m_name = value; }
    public AppConstants.TipoPokemon TipoDeAtaque { get => m_tipoDeAtaque; set => m_tipoDeAtaque = value; }

    public override IEnumerator Effect(TrainerParent trainer)
    {
        print(trainer.CurrentPokemonPicked + " utilizó " + m_name);
        yield return new WaitForSeconds(0.5f);
        if (CheckIfHit())
        {
            float stabMod = 1f;
            if(trainer == CombatManager.Instance.Player)
            {
                //HACER QUE EL DAÑO SE CALCULE POR METODOS y TERMINAR FORMULA
                float dmg = m_damage * (trainer.CurrentPokemonPicked.Attack/CombatManager.Instance.Enemy.CurrentPokemonPicked.Defense)
                    * stabMod * Random.Range(0.85f, 1f);
                print(dmg);
                //CombatManager.Instance.Enemy.CurrentPokemonPicked.DealDamage(dmg);
            }
            else
            {
                stabMod = 1f;
                float dmg = m_damage * (trainer.CurrentPokemonPicked.Attack / CombatManager.Instance.Player.CurrentPokemonPicked.Defense)
                    * stabMod * Random.Range(0.85f, 1f);
                print(dmg);
            }
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            print("El ataque ha fallado");
        }
    }
    

    public bool CheckIfHit()
    {
        float random = Random.Range(0f, 1f);
        if (random <= m_accuracy)
        {
            return true;
        }
        else
        {
            return false;
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
