using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New trainer", menuName = "Trainer", order = 2)]
public class TrainerSO : ScriptableObject
{
    public List<SOPokemonStats> pokemonTeam;
}
