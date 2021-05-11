using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCrow.Fluid.BTs.Samples;

public class CombatManager : PersistentSingleton<CombatManager>
{

    private enum PhaseOfBattle { Start, PickingAction, SettingUpOrder, Acting}

    private PhaseOfBattle m_currentPhase = PhaseOfBattle.Start;

    public enum ActionType { Attack, UseItem, SwapPokemon}

    [SerializeField] private TrainerParent m_player;

    private EnemyTrainerIA m_enemy;

    private int trainersReady = 0;


    public TrainerParent trainerThatActsFirst;
    public TrainerParent trainerThatActsSecond;

    public TrainerParent Player { get => m_player;}

    public void HasPicked()
    {
        m_enemy.Act();
        SetUpOrder();
        StartCoroutine(Act());
    }

    public IEnumerator Act()
    {
        //Change UI
        yield return new WaitForSeconds(1f);
        trainerThatActsFirst.ActionChosen.StartCoroutine(trainerThatActsFirst.ActionChosen.Effect());
        yield return new WaitForSeconds(1f);
        trainerThatActsSecond.ActionChosen.StartCoroutine(trainerThatActsSecond.ActionChosen.Effect());
        yield return new WaitForSeconds(1f);
        //Show UI Again

    }

    public void SetUpOrder()
    {
        trainerThatActsFirst = null;
        trainerThatActsFirst = null;

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
        if(m_player.CurrentPokemonPicked.Speed > m_enemy.CurrentPokemonPicked.Speed)
        {
            trainerThatActsFirst = m_player;
            trainerThatActsSecond = m_enemy;
        }
        else if(m_player.CurrentPokemonPicked.Speed < m_enemy.CurrentPokemonPicked.Speed)
        {
            trainerThatActsFirst = m_enemy;
            trainerThatActsSecond = m_player;
        }
        else
        {
            if(Random.value <= 0.5f){
                trainerThatActsFirst = m_player;
                trainerThatActsSecond = m_enemy;
            }
            else{
                trainerThatActsFirst = m_enemy;
                trainerThatActsSecond = m_player;
            }
        }
    }

}
