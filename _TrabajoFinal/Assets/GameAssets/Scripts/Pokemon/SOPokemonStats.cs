using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

    public TMParent _tm1;
    public TMParent _tm2;
    public TMParent _tm3;
    public TMParent _tm4;
}
