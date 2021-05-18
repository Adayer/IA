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

    private TMParent m_tm1;
    private TMParent m_tm2;
    private TMParent m_tm3;
    private TMParent m_tm4;

    public int Speed { get => m_speed; }
    public string Name { get => m_name; set => m_name = value; }
    public AppConstants.TipoPokemon Type { get => m_type; set => m_type = value; }
    public Sprite Sprite { get => m_sprite; set => m_sprite = value; }

    public TMParent Tm1 { get => m_tm1; set => m_tm1 = value; }
    public TMParent Tm2 { get => m_tm2; set => m_tm2 = value; }
    public TMParent Tm3 { get => m_tm3; set => m_tm3 = value; }
    public TMParent Tm4 { get => m_tm4; set => m_tm4 = value; }
    public int Attack { get => m_attack; set => m_attack = value; }
    public int Defense { get => m_defense; set => m_defense = value; }
    public int SpAtt { get => m_spAtt; set => m_spAtt = value; }
    public int SpDef { get => m_spDef; set => m_spDef = value; }
    public int Speed1 { get => m_speed; set => m_speed = value; }

    public void LoseLife()
    {

    }

    public TMParent CalculateBestMove()
    {
        TMParent bestMove = null;



        return bestMove;
    }
    public void SetProperties(SOPokemonStats pkmData)
    {
        m_name = pkmData._name;
        m_type = pkmData._type;
        m_maxHp = pkmData._maxHp;
        m_currentHP = m_maxHp;
        m_attack = pkmData._attack;
        m_defense = pkmData._defense;
        m_spAtt = pkmData._spAtt;
        m_spDef = pkmData._spDef;
        m_speed = pkmData._speed;
        m_sprite = pkmData._sprite;

        GameObject newTM1= Instantiate(new GameObject(), Vector3.zero, Quaternion.identity, this.transform);
        m_tm1 = newTM1.AddComponent<TMParent>();
        m_tm1.SetProperties(pkmData._tm1);
        newTM1.name = m_tm1.Name;

        GameObject newTM2 = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity, this.transform);
        m_tm2 = newTM2.AddComponent<TMParent>();
        m_tm2.SetProperties(pkmData._tm2);
        newTM2.name = m_tm2.Name;

        GameObject newTM3 = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity, this.transform);
        m_tm3 = newTM3.AddComponent<TMParent>();
        m_tm3.SetProperties(pkmData._tm3);
        newTM3.name = m_tm3.Name;

        GameObject newTM4 = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity, this.transform);
        m_tm4 = newTM4.AddComponent<TMParent>();
        m_tm4.SetProperties(pkmData._tm4);
        newTM4.name = m_tm4.Name;
    }

    public void DealDamage (float amount)
    {

    }
}
