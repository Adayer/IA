using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New pokemon", menuName = "Pokemon", order = 1)]
public class SOPokemonStats : ScriptableObject
{
    public string _name;

    public AppConstants.TipoPokemon _type;
    
    public int _maxHp;
    public int _attack;
    public int _defense;
    public int _spAtt;
    public int _spDef;
    public int _speed;

    public Sprite _sprite;

    public TMSO _tm0;
    public TMSO _tm1;
    public TMSO _tm2;
    public TMSO _tm3;
}
