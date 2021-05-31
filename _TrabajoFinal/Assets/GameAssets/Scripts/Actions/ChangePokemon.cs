using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePokemon : ActionParent
{
    private PokemonParent m_pokemon;
    [SerializeField] private bool m_isPlayerButton = true;

    public PokemonParent Pokemon
    {
        get => m_pokemon;
        set
        {
            if (m_pokemon != null && m_isPlayerButton)
                m_pokemon.OnPokemonPlayerFainted -= MakeUninteractable;
            m_pokemon = value;
            if(m_isPlayerButton)
                m_pokemon.OnPokemonPlayerFainted += MakeUninteractable;
        }
    }
    private void OnDisable()
    {
        if(m_isPlayerButton)
            m_pokemon.OnPokemonPlayerFainted -= MakeUninteractable;
    }
    void MakeUninteractable()
    {
        GetComponent<Button>().interactable = false;
    }

    public override IEnumerator Effect(TrainerParent trainer)
    {
        CombatManager.Instance.Player.UninteractableChangePokemonButtons();
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
