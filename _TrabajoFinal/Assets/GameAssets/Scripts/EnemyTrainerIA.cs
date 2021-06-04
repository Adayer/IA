using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CleverCrow.Fluid.BTs.Samples
{
    /// <summary>
    /// Example script to test out BehaviorTrees, not actually compiled into the released package
    /// </summary>
    [RequireComponent(typeof(ChangePokemon), typeof(HealingPotion))]
    public class EnemyTrainerIA : TrainerParent
    {
        [SerializeField]
        private BehaviorTree _trainerIA;

        ChangePokemon m_changePokemonAction = null;
        float bestPokemonMultiplier = 0f;


        int bestTm = 0;
        int damageOfTm = 0;
        bool canKill = false;

        int bestPokemon = -1;
        HealingPotion m_healingPotion;
        byte m_currentPotion = 1;
        byte m_cooldownCambiar = 3;
        byte m_currentCDCambiar;


        private void Awake()
        {            
            m_changePokemonAction = this.GetComponent<ChangePokemon>();
            m_healingPotion = this.GetComponent<HealingPotion>();
            m_currentPotion = 1;

            _trainerIA = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .Condition("Can KO?", () => CalculateIfCanKill(CurrentPokemonPicked))
                    .Do("Use attack that can KO", () => UseBestAttack())
                .End()
                .Sequence()
                    .Condition("Has low HP?", () => CheckIfLowHP())
                    .Sequence()
                        .Condition("Has healing Items?", () => CheckIfHasHealingItems())
                        .Do("Use healing item", () => UseHealingItem())
                    .End()
                .End()
                .Sequence()
                    .Condition("Is good match up?", () => CheckTypeMatchUp(CurrentPokemonPicked))
                    .Do("Use best Attack", () => UseBestAttack()/*{  print("GOOD MATCHUP, CHOOSING BEST ATTACK attack"); return TaskStatus.Success; }*/ )
                .End()
                .Selector()
                    .Sequence()
                        .Condition("Have better pokemon?", () => CheckIfBetterPokemon())
                        .Do("Change pokemon", () => ChangeToBestPokemon())
                    .End()
                    .Do("Use best Attack", () => UseBestAttack()    /*{print("NOT GOOD MATCHUP, CHOOSING BEST ATTACK attack"); return TaskStatus.Success; }*/ )
                .End()
            .End()
            .Build();
            //print("Tree built");
        }

        public override void UpdatePickedPokemon(PokemonParent newPickedPkmn)
        {
            if(CurrentPokemonPicked != null)
            {
                CurrentPokemonPicked.OnPokemonEnemyFainted -= CombatManager.Instance.StopActEnemyFainted;
            }
            base.UpdatePickedPokemon(newPickedPkmn);
            CurrentPokemonPicked.OnPokemonEnemyFainted += CombatManager.Instance.StopActEnemyFainted;
        }

        public void Act()
        {
            if(m_currentCDCambiar > 0)
            {
                m_currentCDCambiar--;
            }
            _trainerIA.Tick();
        }
        public IEnumerator SendNewPokemon()
        {
            m_currentPickedPokemon.HasFainted = true;
            ChooseBestPokemon();
            if(bestPokemon == -1)
            {
                CombatManager.Instance.OnLastPokemonFaint?.Invoke();
            }
            else if(bestPokemon >= 0)
            {
                StartCoroutine(SwapPokemon());
            }
            //m_changePokemonAction.Pokemon = m_pokemonTeam[bestPokemon];
            ChooseAction(m_changePokemonAction, CombatManager.ActionType.SwapPokemon);
            yield return new WaitForSeconds(1f);
            if(bestPokemon == -1 && m_currentPickedPokemon.CurrentHP <= 0)
            {
                CombatManager.Instance.EnemyTrainerLost();
            }
        }

        private IEnumerator SwapPokemon()
        {
            print("El " + CurrentPokemonPicked.Name + " enemigo ha sido derrotado.");
            yield return new WaitForSeconds(1f);
            UpdatePickedPokemon(m_pokemonTeam[bestPokemon]);
            print("El enemigo lanza a " + CurrentPokemonPicked.Name);
        }

        //Se llama a este metodo cuando un pokemon cae insconsciente y quedan pokemons
        private void ChooseBestPokemon()
        {
            bestPokemon = -1;
            for (int i = 0; i < m_pokemonTeam.Count; i++)
            {
                if (!m_pokemonTeam[i].HasFainted)
                {
                    if (bestPokemon == -1)
                    {
                        bestPokemon = i;
                        bestPokemonMultiplier = PokemonParent.GetTypeDamageMultiplier(m_pokemonTeam[i].Type, CombatManager.Instance.Player.CurrentPokemonPicked.Type);
                    }
                    else
                    {
                        float pokemonMultiplier = PokemonParent.GetTypeDamageMultiplier(m_pokemonTeam[i].Type, CombatManager.Instance.Player.CurrentPokemonPicked.Type);

                        if (pokemonMultiplier == 2)
                        {
                            if(bestPokemonMultiplier == 2)
                            {
                                if (Random.value >= 0.5f)
                                {
                                    bestPokemon = i;
                                    bestPokemonMultiplier = PokemonParent.GetTypeDamageMultiplier(m_pokemonTeam[i].Type, CombatManager.Instance.Player.CurrentPokemonPicked.Type);
                                }
                            }
                            else
                            {
                                bestPokemon = i;
                                bestPokemonMultiplier = PokemonParent.GetTypeDamageMultiplier(m_pokemonTeam[i].Type, CombatManager.Instance.Player.CurrentPokemonPicked.Type);
                            }
                            continue;
                        }
                        else if (pokemonMultiplier == 1)
                        {
                            if(bestPokemonMultiplier == 1)
                            {
                                if (Random.value >= 0.5f)
                                {
                                    bestPokemon = i;
                                    bestPokemonMultiplier = PokemonParent.GetTypeDamageMultiplier(m_pokemonTeam[i].Type, CombatManager.Instance.Player.CurrentPokemonPicked.Type);
                                }
                            }
                            if(bestPokemonMultiplier == 0.5f)
                            {
                                bestPokemon = i;
                                bestPokemonMultiplier = PokemonParent.GetTypeDamageMultiplier(m_pokemonTeam[i].Type, CombatManager.Instance.Player.CurrentPokemonPicked.Type);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (pokemonMultiplier == 0.5f)
                        {
                            if (bestPokemonMultiplier == 0.5f)
                            {
                                if (Random.value >= 0.5f)
                                {
                                    bestPokemon = i;
                                    bestPokemonMultiplier = PokemonParent.GetTypeDamageMultiplier(m_pokemonTeam[i].Type, CombatManager.Instance.Player.CurrentPokemonPicked.Type);
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }
                
        private bool CalculateIfCanKill(PokemonParent pokemon)
        {
            bestTm = -1;
            damageOfTm = 0;
            canKill = false;

            for (int i = 0; i < CurrentPokemonPicked.Tms.Count; i++)
            {
                float damageCalc = 0;
                CombatManager.Instance.OnCalculateDamage?.Invoke(CurrentPokemonPicked.Tms[i], pokemon, CombatManager.Instance.Player.CurrentPokemonPicked, ref damageCalc);

                int flooredCalc = Mathf.FloorToInt(damageCalc);

                if (flooredCalc >= CombatManager.Instance.Player.CurrentPokemonPicked.CurrentHP)
                {
                    if(bestTm == -1)
                    {
                        bestTm = i;
                        damageOfTm = flooredCalc;
                        canKill = true;
                    }

                    if (CurrentPokemonPicked.Tms[i].Accuracy > CurrentPokemonPicked.Tms[bestTm].Accuracy)
                    {
                        bestTm = i;
                        damageOfTm = flooredCalc;
                        canKill = true;
                    }
                }
            }
            return canKill;
        }

        private bool CheckIfLowHP()
        {
            if (CurrentPokemonPicked.CurrentHP <= CurrentPokemonPicked.MaxHp * 0.3f)
            {
                return true;
            }
            return false;
        }

        private bool CheckIfHasHealingItems()
        {
            print(m_currentPotion);
            if(m_currentPotion > 0)
            {
                Debug.LogError("I could heal");
                return true;
            }
            return false;
        }

        private bool CheckTypeMatchUp(PokemonParent pokemonToCheck) // Returns true is supereffective or neutral
        {
            bool isGood = false;
            if (pokemonToCheck.Type == AppConstants.TipoPokemon.Normal
                || CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Normal)
            {
                isGood = true;
            }
            else
            {
                switch (pokemonToCheck.Type)
                {
                    case AppConstants.TipoPokemon.Fire:
                        {
                            if (CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Fire
                                || CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Grass)
                            {
                                isGood = true;
                            }
                            else
                            {
                                isGood = false;
                            }
                        }
                        break;
                    case AppConstants.TipoPokemon.Water:
                        {
                            if (CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Fire
                                || CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Water)
                            {
                                //print("is good matchup");
                                isGood = true;
                            }
                            else
                            {
                                isGood = false;
                            }
                        }
                        break;
                    case AppConstants.TipoPokemon.Grass:
                        {
                            if (CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Grass
                                || CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Water)
                            {
                                isGood = true;
                            }
                            else
                            {
                                isGood = false;
                            }
                        }
                        break;
                }
            }
            
            return isGood;
        }

        private bool CheckIfBetterPokemon()
        {
            if(m_currentCDCambiar > 0)
            {
                return false;
            }
            bool bestPokemonCanKill = false;

            bestPokemon = -1;

            //Debug.LogError(CurrentPokemonPicked.Type);

            for (int i = 0; i < m_pokemonTeam.Count; i++)
            {
                if (m_pokemonTeam[i].HasFainted)
                {
                    continue;
                }
                if (m_pokemonTeam[i] == CurrentPokemonPicked)
                {
                    continue;
                }
                if (m_pokemonTeam[i].Type == CurrentPokemonPicked.Type)
                {
                    continue;
                }
               
                bool canKill = CalculateIfCanKill(m_pokemonTeam[i]);

                if(CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Normal)
                {
                    if (bestPokemon == -1)
                    {
                        if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].MaxHp * 0.5f)
                        {
                            bestPokemon = i;
                            bestPokemonCanKill = canKill;
                        }
                    }
                    else
                    {
                        if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].MaxHp * 0.5f)
                        {
                            if (bestPokemonCanKill)
                            {
                                if (canKill)
                                {
                                    if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                                && m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[bestPokemon].CurrentHP)
                                        {
                                            bestPokemon = i;
                                            bestPokemonCanKill = canKill;
                                        }
                                    }
                                    else if (m_pokemonTeam[i].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                        && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[bestPokemon].CurrentHP)
                                        {
                                            bestPokemon = i;
                                            bestPokemonCanKill = canKill;
                                        }
                                    }
                                    else if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                        && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        bestPokemon = i;
                                        bestPokemonCanKill = canKill;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (canKill)
                                {
                                    bestPokemon = i;
                                    bestPokemonCanKill = canKill;
                                }
                                else
                                {
                                    if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                                && m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[bestPokemon].CurrentHP)
                                        {
                                            bestPokemon = i;
                                            bestPokemonCanKill = canKill;
                                        }
                                    }
                                    else if (m_pokemonTeam[i].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                        && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[bestPokemon].CurrentHP)
                                        {
                                            bestPokemon = i;
                                            bestPokemonCanKill = canKill;
                                        }
                                    }
                                    else if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                        && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        bestPokemon = i;
                                        bestPokemonCanKill = canKill;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (m_pokemonTeam[i].Type != AppConstants.TipoPokemon.Normal)
                {
                    if (m_pokemonTeam[i].Type != CombatManager.Instance.Player.CurrentPokemonPicked.Type)
                    {
                        if (bestPokemon == -1)
                        {
                            if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].MaxHp * 0.5f)
                            {
                                bestPokemon = i;
                                bestPokemonCanKill = canKill;
                            }
                        }
                        else
                        {
                            if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].MaxHp * 0.5f)
                            {
                                if (bestPokemonCanKill)
                                {
                                    if (!canKill)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].MaxHp * 0.5f)
                                        {
                                            if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                                && m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                            {
                                                if (m_pokemonTeam[i].Type != m_pokemonTeam[bestPokemon].Type)
                                                {
                                                    if (m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
                                                    {
                                                        if (m_pokemonTeam[i].CurrentHP * 1.5f > m_pokemonTeam[bestPokemon].CurrentHP * 0.75f)
                                                        {
                                                            bestPokemon = i;
                                                            bestPokemonCanKill = canKill;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (m_pokemonTeam[i].CurrentHP * 1.5f > m_pokemonTeam[bestPokemon].CurrentHP)
                                                        {
                                                            bestPokemon = i;
                                                            bestPokemonCanKill = canKill;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
                                                    {
                                                        if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                                        {
                                                            bestPokemon = i;
                                                            bestPokemonCanKill = canKill;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (m_pokemonTeam[i].CurrentHP * 0.75f > m_pokemonTeam[bestPokemon].CurrentHP)
                                                        {
                                                            bestPokemon = i;
                                                            bestPokemonCanKill = canKill;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (m_pokemonTeam[i].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                                && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                            {
                                                if (m_pokemonTeam[i].Type != m_pokemonTeam[bestPokemon].Type)
                                                {
                                                    if (m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
                                                    {
                                                        if (m_pokemonTeam[i].CurrentHP * 1.5f > m_pokemonTeam[bestPokemon].CurrentHP * 0.75f)
                                                        {
                                                            bestPokemon = i;
                                                            bestPokemonCanKill = canKill;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (m_pokemonTeam[i].CurrentHP * 1.5f > m_pokemonTeam[bestPokemon].CurrentHP)
                                                        {
                                                            bestPokemon = i;
                                                            bestPokemonCanKill = canKill;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
                                                    {
                                                        if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                                        {
                                                            bestPokemon = i;
                                                            bestPokemonCanKill = canKill;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (m_pokemonTeam[i].CurrentHP * 0.75f > m_pokemonTeam[bestPokemon].CurrentHP)
                                                        {
                                                            bestPokemon = i;
                                                            bestPokemonCanKill = canKill;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                                && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                            {
                                                if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].MaxHp * 0.5f)
                                                {
                                                    bestPokemon = i;
                                                    bestPokemonCanKill = canKill;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (canKill)
                                    {
                                        bestPokemon = i;
                                        bestPokemonCanKill = canKill;
                                    }
                                    else
                                    {
                                        if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                            && m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                        {
                                            if (m_pokemonTeam[i].Type != m_pokemonTeam[bestPokemon].Type)
                                            {
                                                if (m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
                                                {
                                                    if (m_pokemonTeam[i].CurrentHP * 1.5f > m_pokemonTeam[bestPokemon].CurrentHP * 0.75f)
                                                    {
                                                        bestPokemon = i;
                                                        bestPokemonCanKill = canKill;
                                                    }
                                                }
                                                else
                                                {
                                                    if (m_pokemonTeam[i].CurrentHP * 1.5f > m_pokemonTeam[bestPokemon].CurrentHP)
                                                    {
                                                        bestPokemon = i;
                                                        bestPokemonCanKill = canKill;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
                                                {
                                                    if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                                    {
                                                        bestPokemon = i;
                                                        bestPokemonCanKill = canKill;
                                                    }
                                                }
                                                else
                                                {
                                                    if (m_pokemonTeam[i].CurrentHP * 0.75f > m_pokemonTeam[bestPokemon].CurrentHP)
                                                    {
                                                        bestPokemon = i;
                                                        bestPokemonCanKill = canKill;
                                                    }
                                                }
                                            }
                                        }
                                        else if (m_pokemonTeam[i].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                            && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                        {
                                            if (m_pokemonTeam[i].Type != m_pokemonTeam[bestPokemon].Type)
                                            {
                                                if (m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
                                                {
                                                    if (m_pokemonTeam[i].CurrentHP * 1.5f > m_pokemonTeam[bestPokemon].CurrentHP * 0.75f)
                                                    {
                                                        bestPokemon = i;
                                                        bestPokemonCanKill = canKill;
                                                    }
                                                }
                                                else
                                                {
                                                    if (m_pokemonTeam[i].CurrentHP * 1.5f > m_pokemonTeam[bestPokemon].CurrentHP)
                                                    {
                                                        bestPokemon = i;
                                                        bestPokemonCanKill = canKill;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
                                                {
                                                    if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                                    {
                                                        bestPokemon = i;
                                                        bestPokemonCanKill = canKill;
                                                    }
                                                }
                                                else
                                                {
                                                    if (m_pokemonTeam[i].CurrentHP * 0.75f > m_pokemonTeam[bestPokemon].CurrentHP)
                                                    {
                                                        bestPokemon = i;
                                                        bestPokemonCanKill = canKill;
                                                    }
                                                }
                                            }
                                        }
                                        else if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                            && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                        {
                                            if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].MaxHp * 0.5f)
                                            {
                                                bestPokemon = i;
                                                bestPokemonCanKill = canKill;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (m_pokemonTeam[i].Type == AppConstants.TipoPokemon.Normal || m_pokemonTeam[i].Type == CombatManager.Instance.Player.CurrentPokemonPicked.Type)
                {
                    if (bestPokemon == -1)
                    {
                        if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].MaxHp * 0.5f)
                        {
                            bestPokemon = i;
                            bestPokemonCanKill = canKill;
                        }
                    }
                    else
                    {
                        if (m_pokemonTeam[bestPokemon].Type != AppConstants.TipoPokemon.Normal
                            && m_pokemonTeam[bestPokemon].Type != CurrentPokemonPicked.Type
                            && m_pokemonTeam[bestPokemon].Type != CombatManager.Instance.Player.CurrentPokemonPicked.Type)
                        {
                            continue;
                        }
                        else
                        {
                            if (bestPokemonCanKill)
                            {
                                if (!canKill)
                                {
                                    continue;
                                }
                                else
                                {
                                    if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                        && m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        if (m_pokemonTeam[bestPokemon].Type != CurrentPokemonPicked.Type)
                                        {
                                            if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                            {
                                                bestPokemon = i;
                                                bestPokemonCanKill = canKill;
                                            }
                                        }
                                        else
                                        {
                                            bestPokemon = i;
                                            bestPokemonCanKill = canKill;
                                        }
                                    }
                                    else if (m_pokemonTeam[i].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                        && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        if (m_pokemonTeam[bestPokemon].Type != CurrentPokemonPicked.Type)
                                        {
                                            if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                            {
                                                bestPokemon = i;
                                                bestPokemonCanKill = canKill;
                                            }
                                        }
                                        else
                                        {
                                            bestPokemon = i;
                                            bestPokemonCanKill = canKill;
                                        }
                                    }
                                    else if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                        && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        bestPokemon = i;
                                        bestPokemonCanKill = canKill;
                                    }
                                }
                            }
                            else
                            {
                                if (!canKill)
                                {
                                    if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                        && m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        if (m_pokemonTeam[bestPokemon].Type != CurrentPokemonPicked.Type)
                                        {
                                            if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                            {
                                                bestPokemon = i;
                                                bestPokemonCanKill = canKill;
                                            }
                                        }
                                        else
                                        {
                                            bestPokemon = i;
                                            bestPokemonCanKill = canKill;
                                        }
                                    }
                                    else if (m_pokemonTeam[i].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                        && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        if (m_pokemonTeam[bestPokemon].Type != CurrentPokemonPicked.Type)
                                        {
                                            if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                            {
                                                bestPokemon = i;
                                                bestPokemonCanKill = canKill;
                                            }
                                        }
                                        else
                                        {
                                            bestPokemon = i;
                                            bestPokemonCanKill = canKill;
                                        }
                                    }
                                    else if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                        && m_pokemonTeam[bestPokemon].Speed < CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                    {
                                        bestPokemon = i;
                                        bestPokemonCanKill = canKill;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            //Debug.LogError(bestPokemon);
            if (bestPokemon == -1)
            {
                return false;
            }
            m_currentCDCambiar = m_cooldownCambiar;
            return true;
        }

        private CleverCrow.Fluid.BTs.Tasks.TaskStatus UseBestAttack()
        {
            if (!canKill)
            {
                float damageCalc = 0;
                int bestDamage = 0;
                for (int i = 0; i < CurrentPokemonPicked.Tms.Count; i++)
                {
                    CombatManager.Instance.OnCalculateDamage?.Invoke(CurrentPokemonPicked.Tms[i], CurrentPokemonPicked, CombatManager.Instance.Player.CurrentPokemonPicked, ref damageCalc);
                    //print("Damage calculation is equal to" + damageCalc);
                    int flooredCalc = Mathf.FloorToInt(damageCalc);
                    if (flooredCalc >= bestDamage)
                    {
                        bestTm = i;
                        bestDamage = flooredCalc;
                      
                    }
                    damageCalc = 0;
                }
                damageOfTm = bestDamage;
            }

            ChooseAction(CurrentPokemonPicked.Tms[bestTm], CombatManager.ActionType.Attack);
            //Debug.LogError("Chose " + CurrentPokemonPicked.Tms[bestTm].Name + " as best attack with type " + CurrentPokemonPicked.Tms[bestTm].TipoDeAtaque);
            return TaskStatus.Success;
        }
        private CleverCrow.Fluid.BTs.Tasks.TaskStatus UseHealingItem()
        {
            ChooseAction(m_healingPotion, CombatManager.ActionType.UseItem);
            m_currentPotion--;
            return TaskStatus.Success;
        }
        private CleverCrow.Fluid.BTs.Tasks.TaskStatus ChangeToBestPokemon()
        {
            Debug.LogWarning("Swaping to " + m_pokemonTeam[bestPokemon].Name + ", Current Picked pokemon is " + m_pokemonTeam[bestPokemon].Name);
            if (bestPokemon == -1)
            {
                return TaskStatus.Failure;
            }
            if (m_pokemonTeam[bestPokemon] == m_currentPickedPokemon)
            {
                return TaskStatus.Failure;
            }
            //Debug.LogError(m_changePokemonAction.name);
            m_changePokemonAction.Pokemon = m_pokemonTeam[bestPokemon];
            ChooseAction(m_changePokemonAction, CombatManager.ActionType.SwapPokemon);
            return TaskStatus.Success;
        }
    }
}
