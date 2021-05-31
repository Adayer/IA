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
    List<TrainerSO> m_enemyTrainerListSO;
    GenericQueue<EnemyTrainerIA> m_enemyTrainerQueue;
    [SerializeField] int m_numberOfEnemyTrainers;
    bool m_playerSendingOutNewPokemon = false;
    int m_currentEnemyTrainerIndex;
    public PlayerTrainer Player { get => m_player; }
    public EnemyTrainerIA Enemy
    {
        get => m_enemy;
        set
        {
            m_enemy = value;
            //value.Initialize();
            OnEnemyTrainerChanged?.Invoke(this, new NewTrainerArgs(m_enemyTrainerListSO[m_currentEnemyTrainerIndex]));
        }
    }

    public delegate void DamageCalculation(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon, ref float totalD);

    public DamageCalculation OnDealDamage;
    public DamageCalculation OnCalculateDamage;

    public event EventHandler<NewTrainerArgs> OnEnemyTrainerChanged;

    public delegate void CombatIsFinished();
    public CombatIsFinished OnLastPokemonFaint;
    public void HasPicked()
    {
        m_enemy.Act();
        SetUpOrder();
        StartCoroutine(Act());
    }

    [SerializeField] GameObject m_ActionsPanel;

    public IEnumerator Act()
    {
        print("-------");

        Player.DisableUI();
        if (m_playerSendingOutNewPokemon)
        {
            Player.ActionChosen.StartCoroutine(Player.ActionChosen.Effect(Player));
            m_playerSendingOutNewPokemon = false;
        }
        else
        {
            //Change UI
            //m_ActionsPanel.transform.position -= Vector3.up*500f;
            yield return new WaitForSeconds(2f);
            trainerThatActsFirst.ActionChosen.StartCoroutine(trainerThatActsFirst.ActionChosen.Effect(trainerThatActsFirst));
            yield return new WaitForSeconds(2f);
            trainerThatActsSecond.ActionChosen.StartCoroutine(trainerThatActsSecond.ActionChosen.Effect(trainerThatActsSecond));
            //Show UI Again
        }
        yield return new WaitForSeconds(2f);
        Player.EnableUI();
        Player.UpdatePokemonTeam();
    }

    public void StopActEnemyFainted()
    {
        StopAllCoroutines();
        StartCoroutine(IASendNewPokemon());
    }

    public void StopActPlayerFainted()
    {
        StopAllCoroutines();
        print("Elige un pokemon");
        Player.EnableChangePokemonButtons();
        Player.UpdatePokemonTeam();
        m_playerSendingOutNewPokemon = true;
    }

    private IEnumerator IASendNewPokemon()
    {
        StartCoroutine(m_enemy.SendNewPokemon());
        yield return new WaitForSeconds(1f);
        Player.EnableUI();
    }

    private void OnEnable()
    {
        OnDealDamage += CalculateBaseDamage;
        OnDealDamage += CalculateSTABDamage;
        OnDealDamage += CalculateTypeCounterDamage;
        OnDealDamage += CalculateCritChance;
        OnDealDamage += CalculateMissChance;
        OnDealDamage += DealDamage;

        OnEnemyTrainerChanged += SetNewEnemyTrainer;

        OnCalculateDamage += CalculateBaseMinDamage;
        OnCalculateDamage += CalculateSTABDamage;
        OnCalculateDamage += CalculateTypeCounterDamage;
    }

    private void SetNewEnemyTrainer(object sender, NewTrainerArgs trainerArgs)
    {
        Enemy.Initialize(trainerArgs.newEnemyTrainer.pokemonTeam);
        Player.HealAllPokemon();
    }

    private void OnDisable()
    {
        OnDealDamage -= CalculateBaseDamage;
        OnDealDamage -= CalculateSTABDamage;
        OnDealDamage -= CalculateTypeCounterDamage;
        OnDealDamage -= CalculateCritChance;
        OnDealDamage -= CalculateMissChance;
        OnDealDamage -= DealDamage;

        OnEnemyTrainerChanged -= SetNewEnemyTrainer;

        OnCalculateDamage -= CalculateBaseMinDamage;
        OnCalculateDamage -= CalculateSTABDamage;
        OnCalculateDamage -= CalculateTypeCounterDamage;
    }
    private void CalculateBaseDamage(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon, ref float totalDamage)
    {
        int attackValue;
        int defenseValue;
        totalDamage = 0;
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
        totalDamage = Random.Range(0.85f, 1) * tmUsed.Damage * attackValue / defenseValue;
    }

    private void CalculateBaseMinDamage(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon, ref float totalDamage)
    {
        int attackValue;
        int defenseValue;
        totalDamage = 0;
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
        totalDamage = 0.85f * tmUsed.Damage * attackValue / defenseValue;
    }

    private void CalculateSTABDamage(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon, ref float totalDamage)
    {
        if (attackingPokemon.Type == tmUsed.TipoDeAtaque)
        {
            totalDamage *= 1.2f;
        }
    }

    private void CalculateTypeCounterDamage(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon, ref float totalDamage)
    {
        totalDamage *= PokemonParent.GetTypeDamageMultiplier(tmUsed.TipoDeAtaque, defendingPokemon.Type);
    }

    private void CalculateCritChance(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon, ref float totalDamage)
    {
        if (Random.value * 100 > tmUsed.CritChance)
        {
            totalDamage *= 1.5f;
        }
    }

    private void CalculateMissChance(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon, ref float totalDamage)
    {
        if (Random.value >= tmUsed.Accuracy)
        {
            totalDamage = 0;
        }
    }

    private void DealDamage(TMParent tmUsed, PokemonParent attackingPokemon, PokemonParent defendingPokemon, ref float totalDamage)
    {
        defendingPokemon.TakeDamage(Mathf.FloorToInt(totalDamage));
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
        m_currentEnemyTrainerIndex++;
        m_enemyTrainerQueue.QuitarDeLaFila();
        print("HAS GANADO A UN ENTRENADOR! " + m_enemyTrainerQueue.count + " restante(s). Curando a todos tus pokémon");
        if (m_enemyTrainerQueue.count > 0)
            Enemy = m_enemyTrainerQueue.head.data;
        else
            print("ENHORABUENA! HAS GANADO A TODOS LOS ENTRENAORES!");
    }

    public void StartCombat()
    {
        Player.Initialize(null);

        if (m_enemyTrainerQueue == null)
            m_enemyTrainerQueue = new GenericQueue<EnemyTrainerIA>();
        GameObject trainers = new GameObject("Trainers");
        int remaining = m_enemyTrainerListSO.Count;
        int i = 0;
        while (remaining > 0)
        {
            //Debug.LogError("Remaining trainers to load from SOs" + remaining);
            i++;
            EnemyTrainerIA trainer = new GameObject("Trainer" + (i)).AddComponent<EnemyTrainerIA>();
            TrainerSO tso = m_enemyTrainerListSO[i-1];
            //Debug.LogError("Loading trainer SO" + tso);
            trainer.InitPokemons(tso.pokemonTeam);
            //Debug.LogError("Initialized pokemons of trainer " + (i));
            trainer.transform.parent = trainers.transform;
            m_enemyTrainerQueue.PonerALaFila(trainer);
            remaining--;
        }
        //Debug.LogError("Ended trainer queue");
        Enemy = m_enemyTrainerQueue.head.data;
        Enemy.SetPokemons(m_enemyTrainerQueue.head.data.PokemonTeam);
    }

    private void Start()
    {
        //m_enemyTrainerQueueSO

        object[] enemyTrainers;
        enemyTrainers = Resources.LoadAll("Trainer", typeof(TrainerSO));
        m_enemyTrainerListSO = new List<TrainerSO>();
        for (int i = 0; i < m_numberOfEnemyTrainers; i++)
        {
            m_enemyTrainerListSO.Add((TrainerSO)enemyTrainers[i]);
        }
    }

}
