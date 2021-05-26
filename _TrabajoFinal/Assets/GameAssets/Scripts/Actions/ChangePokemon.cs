using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePokemon : ActionParent
{
    private PokemonParent m_pokemon;

    public PokemonParent Pokemon { get => m_pokemon; set => m_pokemon = value; }

    public override IEnumerator Effect(TrainerParent attacker)
    {
        print(attacker.CurrentPokemonPicked.Name + " vuelve aquí.");
        yield return new WaitForSeconds(0.5f);
        print(attacker.CurrentPokemonPicked.Name + " te elijo a ti!");
        attacker.CurrentPokemonPicked = m_pokemon;
        attacker.UpdatePickedPokemon(m_pokemon);
        attacker.UpdatePokemonTeam();
    }
}
