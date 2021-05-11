using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonParent : MonoBehaviour
{
    private string m_name;

    private AppConstants.TipoPokemon m_type;

    private int m_maxHp;
    private int m_currentHP;

    private int m_attack;
    private int m_defense;
    private int m_spAtt;
    private int m_spDef;
    private int m_speed;

    private Sprite m_sprite;

    private TMParent _tm1;
    private TMParent _tm2;
    private TMParent _tm3;
    private TMParent _tm4;

    public int Speed { get => m_speed; }

    public PokemonParent(SOPokemonStats pkmData)
    {
        m_name = pkmData._name;
        m_type = pkmData._type;
        m_maxHp = pkmData._maxHp;
        m_currentHP = m_maxHp;
        m_attack= pkmData._attack;
        m_defense = pkmData._defense;
        m_spAtt = pkmData._spAtt;
        m_spDef = pkmData._spDef;
        m_speed = pkmData._speed;
        m_sprite = pkmData._sprite;
        _tm1 = pkmData._tm1;
        _tm2 = pkmData._tm2;
        _tm3 = pkmData._tm3;
        _tm4 = pkmData._tm4;
    }

    public void LoseLife()
    {

    }

    public TMParent CalculateBestMove()
    {
        TMParent bestMove = null;



        return bestMove;
    }
}
