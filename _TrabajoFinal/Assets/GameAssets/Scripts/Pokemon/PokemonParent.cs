using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PokemonParent : MonoBehaviour
{
    private string m_name;

    private AppConstants.TipoPokemon m_type;

    public Action OnPokemonFainted;

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
    public int CurrentHP { get => m_currentHP; set => m_currentHP = value; }

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

        GameObject newTM1= new GameObject("TM");
        newTM1.transform.parent = this.transform;
        m_tm1 = newTM1.AddComponent<TMParent>();
        m_tm1.SetProperties(pkmData._tm1);
        newTM1.name = m_tm1.Name;

        GameObject newTM2 = new GameObject("TM");
        newTM2.transform.parent = this.transform;
        m_tm2 = newTM2.AddComponent<TMParent>();
        m_tm2.SetProperties(pkmData._tm2);
        newTM2.name = m_tm2.Name;

        GameObject newTM3 = new GameObject("TM");
        newTM3.transform.parent = this.transform;
        m_tm3 = newTM3.AddComponent<TMParent>();
        m_tm3.SetProperties(pkmData._tm3);
        newTM3.name = m_tm3.Name;

        GameObject newTM4 = new GameObject("TM");
        newTM4.transform.parent = this.transform;
        m_tm4 = newTM4.AddComponent<TMParent>();
        m_tm4.SetProperties(pkmData._tm4);
        newTM4.name = m_tm4.Name;
    }

    public void SubscribirTMs()
    {
        CombatManager.Instance.Player.OnTM1 += Tm1.Act;
        CombatManager.Instance.Player.OnTM2 += Tm2.Act;
        CombatManager.Instance.Player.OnTM3 += Tm3.Act;
        CombatManager.Instance.Player.OnTM4 += Tm4.Act;
    }

    public void DesubscribirTMs()
    {
        CombatManager.Instance.Player.OnTM1 -= Tm1.Act;
        CombatManager.Instance.Player.OnTM2 -= Tm2.Act;
        CombatManager.Instance.Player.OnTM3 -= Tm3.Act;
        CombatManager.Instance.Player.OnTM4 -= Tm4.Act;
    }

    public void TakeDamage(int amount)
    {
        Debug.LogError(name + " took " + amount +" damage");
        CurrentHP -= amount;
        if(CurrentHP <= 0)
        {
            Faint();
        }
    }

    public void Faint()
    {
        Debug.LogError(name + " fainted");
        OnPokemonFainted?.Invoke();
    }

    public static float GetTypeDamageMultiplier(AppConstants.TipoPokemon attackType, AppConstants.TipoPokemon defenderType)
    {
        float damageMultiplier = 1;

        if (attackType == AppConstants.TipoPokemon.Normal || defenderType == AppConstants.TipoPokemon.Normal)
            return damageMultiplier;

        if ((int)(attackType) == Mod(((int)defenderType - 1), 3))
        {
            damageMultiplier = 0.5f;
        }
        else if ((int)attackType == ((int)defenderType + 1) % 3)
        {
            damageMultiplier = 2f;
        }
        else if (attackType == defenderType)
            damageMultiplier = 0.5f;

        return damageMultiplier;
    }
    static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }


    public static void DebugTypeEffectiveness()
    {
        Debug.LogError("=========THEORETICALLY INEFFECTIVE============");
        Debug.LogError("Fire vs Water: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Fire, AppConstants.TipoPokemon.Water));
        Debug.LogError("Water vs Grass: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Water, AppConstants.TipoPokemon.Grass));
        Debug.LogError("Grass vs Fire: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Grass, AppConstants.TipoPokemon.Fire));

        Debug.LogError("=========THEORETICALLY EFFECTIVE============");
        Debug.LogError("Fire vs Grass: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Fire, AppConstants.TipoPokemon.Grass));
        Debug.LogError("Water vs Fire: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Water, AppConstants.TipoPokemon.Fire));
        Debug.LogError("Grass vs Water: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Grass, AppConstants.TipoPokemon.Water));

        Debug.LogError("=========NORMAL VS TYPE============");
        Debug.LogError("Normal vs Grass: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Normal, AppConstants.TipoPokemon.Grass));
        Debug.LogError("Normal vs Fire: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Normal, AppConstants.TipoPokemon.Fire));
        Debug.LogError("Normal vs Water: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Normal, AppConstants.TipoPokemon.Water));

        Debug.LogError("=========TYPE VS NORMAL============");
        Debug.LogError("Grass vs Normal: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Grass, AppConstants.TipoPokemon.Normal));
        Debug.LogError("Water vs Normal: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Water, AppConstants.TipoPokemon.Normal));
        Debug.LogError("Fire vs Normal: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Fire, AppConstants.TipoPokemon.Normal));

        Debug.LogError("=========TYPE VS SELF============");
        Debug.LogError("Grass vs Grass: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Grass, AppConstants.TipoPokemon.Grass));
        Debug.LogError("Water vs Water: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Water, AppConstants.TipoPokemon.Water));
        Debug.LogError("Fire vs Fire: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Fire, AppConstants.TipoPokemon.Fire));
        Debug.LogError("Normal vs Normal: " + GetTypeDamageMultiplier(AppConstants.TipoPokemon.Normal, AppConstants.TipoPokemon.Normal));
    }
}
