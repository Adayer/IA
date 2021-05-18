using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TM", menuName = "TM", order = 2)]
public class TMSO : ScriptableObject
{
    public AppConstants.TipoPokemon _tipoDeAtaque;
    public AppConstants.TipoDaño _tipoDeDaño;
    public int _accuracy;
    public int _damage;
    public string _name;
}
