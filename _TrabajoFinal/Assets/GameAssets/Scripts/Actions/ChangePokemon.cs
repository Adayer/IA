using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePokemon : ActionParent
{
    private PokemonParent m_pokemon;

    public PokemonParent Pokemon { get => m_pokemon; set => m_pokemon = value; }

    public override IEnumerator Effect(TrainerParent trainer)
    {
        print(trainer.CurrentPokemonPicked.Name + " vuelve aquí.");
        yield return new WaitForSeconds(0.5f);
        print(trainer.CurrentPokemonPicked.Name + " te elijo a ti!");
        trainer.CurrentPokemonPicked = m_pokemon;
        trainer.UpdatePickedPokemon(m_pokemon);
        trainer.UpdatePokemonTeam();
    }
}
