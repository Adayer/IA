using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePokemon : ActionParent
{
    private PokemonParent m_pokemon;

    public PokemonParent Pokemon
    {
        get => m_pokemon;
        set
        {
            if (m_pokemon != null)
                m_pokemon.OnPokemonFainted -= MakeUninteractable;
            m_pokemon = value;
            m_pokemon.OnPokemonFainted += MakeUninteractable;
        }
    }
    private void OnDisable()
    {
        m_pokemon.OnPokemonFainted -= MakeUninteractable;
    }
    void MakeUninteractable()
    {
        GetComponent<Button>().interactable = false;
    }

    public override IEnumerator Effect(TrainerParent trainer)
    {
        print(trainer.CurrentPokemonPicked.Name + " vuelve aquí.");
        yield return new WaitForSeconds(0.5f);
        trainer.UpdatePickedPokemon(Pokemon);
        print(trainer.CurrentPokemonPicked.Name + " te elijo a ti!");
        //trainer.CurrentPokemonPicked = m_pokemon;
        if (trainer is PlayerTrainer)
        {
            ((PlayerTrainer)trainer).UpdatePokemonTeam();
        }
    }
}
