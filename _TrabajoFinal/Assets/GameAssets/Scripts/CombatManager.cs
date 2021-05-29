using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCrow.Fluid.BTs.Samples;
using System;
using Random = UnityEngine.Random;

public class CombatManager : PersistentSingleton<CombatManager>
{
    public enum ActionType { Attack, UseItem, SwapPokemon }

    [SerializeField] private PlayerTrainer m_player;

    [SerializeField] private EnemyTrainerIA m_enemy;

    public TrainerParent trainerThatActsFirst;
    public TrainerParent trainerThatActsSecond;
    [SerializeField] GenericQueue<EnemyTrainerIA> m_enemyTrainerQueue;
    public PlayerTrainer Player { get => m_player; }
    public EnemyTrainerIA Enemy
    {
        get => m_enemy;
        set
        {
            m_enemy = value;
            value.Initialize();
            OnEnemyTrainerChanged?.Invoke(this,new NewTrainerArgs(value));
        }
    }

    float m_totalDamage;
    public delegate void DamageCalculation(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon);

    public DamageCalculation OnCalculateDamage;

    public event EventHandler<NewTrainerArgs> OnEnemyTrainerChanged;
    public void HasPicked()
    {
        m_enemy.Act();
        SetUpOrder();
        StartCoroutine(Act());
    }

    [SerializeField] GameObject m_ActionsPanel;

    public IEnumerator Act()
    {
        //Change UI
        //m_ActionsPanel.transform.position -= Vector3.up*500f;
        Player.DisableUI();
        yield return new WaitForSeconds(1f);
        trainerThatActsFirst.ActionChosen.StartCoroutine(trainerThatActsFirst.ActionChosen.Effect(trainerThatActsFirst));
        yield return new WaitForSeconds(1f);
        trainerThatActsSecond.ActionChosen.StartCoroutine(trainerThatActsSecond.ActionChosen.Effect(trainerThatActsSecond));
        yield return new WaitForSeconds(1f);
        Player.EnableUI();
        Player.UpdatePokemonTeam();
        //Show UI Again

    }
    private void OnEnable()
    {
        OnCalculateDamage += CalculateBaseDamage;
        OnCalculateDamage += CalculateSTABDamage;
        OnCalculateDamage += CalculateTypeCounterDamage;
        OnCalculateDamage += CalculateCritChance;
        OnCalculateDamage += CalculateMissChance;
        OnCalculateDamage += DealDamage;
        OnEnemyTrainerChanged += SetNewEnemyTrainer;
    }

    private void SetNewEnemyTrainer(object sender, NewTrainerArgs trainerArgs)
    {
        //TODO: Limpiar todos los GO pokemon del entrenador anterior?
        trainerArgs.newEnemyTrainer.Initialize();
    }

    private void OnDisable()
    {
        OnCalculateDamage -= CalculateBaseDamage;
        OnCalculateDamage -= CalculateSTABDamage;
        OnCalculateDamage -= CalculateTypeCounterDamage;
        OnCalculateDamage -= CalculateCritChance;
        OnCalculateDamage -= CalculateMissChance;
    }
    private void CalculateBaseDamage(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon)
    {
        int attackValue;
        int defenseValue;
        if (tmUsed.TipoDeDaño == AppConstants.TipoDaño.Fisico)
        {
            attackValue = attackingPokemon.Attack;
            defenseValue = defendingPokemon.Defense;
        }
        else
        {
            attackValue = attackingPokemon.SpAtt;
            defenseValue = defendingPokemon.SpDef;
        }
        m_totalDamage = Random.Range(0.85f, 1) * tmUsed.Damage * attackValue / defenseValue;
    }

    private void CalculateSTABDamage(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon)
    {
        if (attackingPokemon.Type == tmUsed.TipoDeAtaque)
        {
            m_totalDamage *= 1.2f;
        }
    }

    private void CalculateTypeCounterDamage(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon)
    {        
        m_totalDamage *= PokemonParent.GetTypeDamageMultiplier(tmUsed.TipoDeAtaque, defendingPokemon.Type);
    }

    private void CalculateCritChance(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon)
    {
        if (Random.value * 100 > tmUsed.CritChance)
        {
            m_totalDamage *= 1.5f;
        }
    }

    private void CalculateMissChance(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon)
    {
        if (Random.value >= tmUsed.Accuracy)
        {
            m_totalDamage = 0;
        }
    }

    private void DealDamage(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon)
    {
        defendingPokemon.TakeDamage(Mathf.FloorToInt(m_totalDamage));
    }
    public void SetUpOrder()
    {
        trainerThatActsFirst = null;
        trainerThatActsSecond = null;

        switch (m_player.TypeOfActionChosen)
        {
            case ActionType.Attack:
                {
                    switch (m_enemy.TypeOfActionChosen)
                    {
                        case ActionType.Attack:
                            {
                                TieBreakIniciativa();
                            }
                            break;
                        case ActionType.UseItem:
                            {
                                trainerThatActsFirst = m_enemy;
                                trainerThatActsSecond = m_player;
                            }
                            break;
                        case ActionType.SwapPokemon:
                            {
                                trainerThatActsFirst = m_enemy;
                                trainerThatActsSecond = m_player;
                            }
                            break;
                        default:
                            break;
                    }
                }
                break;
            case ActionType.UseItem:
                {
                    switch (m_enemy.TypeOfActionChosen)
                    {
                        case ActionType.Attack:
                            {
                                trainerThatActsFirst = m_player;
                                trainerThatActsSecond = m_enemy;
                            }
                            break;
                        case ActionType.UseItem:
                            {
                                TieBreakIniciativa();
                            }
                            break;
                        case ActionType.SwapPokemon:
                            {
                                trainerThatActsFirst = m_enemy;
                                trainerThatActsSecond = m_player;
                            }
                            break;
                        default:
                            break;
                    }
                }
                break;
            case ActionType.SwapPokemon:
                {
                    switch (m_enemy.TypeOfActionChosen)
                    {
                        case ActionType.Attack:
                            {
                                trainerThatActsFirst = m_player;
                                trainerThatActsSecond = m_enemy;
                            }
                            break;
                        case ActionType.UseItem:
                            {
                                trainerThatActsFirst = m_player;
                                trainerThatActsSecond = m_enemy;
                            }
                            break;
                        case ActionType.SwapPokemon:
                            {
                                TieBreakIniciativa();
                            }
                            break;
                        default:
                            break;
                    }
                }
                break;
            default:
                break;
        }
    }

    void TieBreakIniciativa()
    {
        if (m_player.CurrentPokemonPicked.Speed > m_enemy.CurrentPokemonPicked.Speed)
        {
            trainerThatActsFirst = m_player;
            trainerThatActsSecond = m_enemy;
        }
        else if (m_player.CurrentPokemonPicked.Speed < m_enemy.CurrentPokemonPicked.Speed)
        {
            trainerThatActsFirst = m_enemy;
            trainerThatActsSecond = m_player;
        }
        else
        {
            if (Random.value <= 0.5f)
            {
                trainerThatActsFirst = m_player;
                trainerThatActsSecond = m_enemy;
            }
            else
            {
                trainerThatActsFirst = m_enemy;
                trainerThatActsSecond = m_player;
            }
        }
    }


    public void EnemyTrainerLost()
    {
        Enemy = m_enemyTrainerQueue.QuitarDeLaFila();
    }

    public void StartCombat()
    {
        Player.Initialize();
        Enemy.Initialize();
    }

}
