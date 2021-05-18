using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePokemon : ActionParent
{
    [SerializeField] private PokemonParent pokemon;
    public override IEnumerator Effect(TrainerParent trainer)
    {
        print(trainer.CurrentPokemonPicked.Name + " vuelve aquí.");
        yield return new WaitForSeconds(0.5f);
        print(trainer.CurrentPokemonPicked.Name + " te elijo a ti!");
        trainer.CurrentPokemonPicked = pokemon;
        trainer.UpdatePickedPokemon(pokemon);
    }
}
