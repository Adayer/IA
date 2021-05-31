using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Samples
{
    /// <summary>
    /// Example script to test out BehaviorTrees, not actually compiled into the released package
    /// </summary>
    [RequireComponent(typeof(ChangePokemon))]
    public class EnemyTrainerIA : TrainerParent
    {
        [SerializeField]
        private BehaviorTree _trainerIA;

        ChangePokemon m_changePokemonAction = null;

        private void Awake()
        {
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
                    .Do("Use best Attack", () => UseBestAttack())
                .End()
                .Selector()
                    .Sequence()
                        .Condition("Have better pokemon?", () => CheckIfBetterPokemon())
                        .Do("Change pokemon", () => ChangeToBestPokemon())
                    .End()
                    .Do("Use best Attack", () => UseBestAttack())
                .End()
            .End()
            .Build();
        }

        public void Act()
        {
            _trainerIA.Tick();
        }

        int bestTm = 0;
        int damageOfTm = 0;
        bool canKill = false;
        private bool CalculateIfCanKill(PokemonParent pokemon)
        {
            bestTm = 0;
            damageOfTm = 0;
            canKill = false;

            for (int i = 0; i < CurrentPokemonPicked.Tms.Count; i++)
            {
                float damageCalc = 0;
                CombatManager.Instance.OnCalculateDamage?.Invoke(CurrentPokemonPicked.Tms[i], pokemon, CombatManager.Instance.Player.CurrentPokemonPicked, ref damageCalc);

                int flooredCalc = Mathf.FloorToInt(damageCalc);

                if (flooredCalc >= CombatManager.Instance.Player.CurrentPokemonPicked.CurrentHP)
                {
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
            if(CurrentPokemonPicked.CurrentHP <= CurrentPokemonPicked.CurrentHP * 0.3f)
            {
                return true;
            }
            return false;
        }

        private bool CheckIfHasHealingItems()
        {
            return false;
        }

        private bool CheckTypeMatchUp(PokemonParent pokemonToCheck) // Returns true is supereffective or neutral
        {
            if(pokemonToCheck.Type == AppConstants.TipoPokemon.Normal
                || CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Normal)
            {
                return true;
            }

            switch (pokemonToCheck.Type)
            {
                case AppConstants.TipoPokemon.Fire:
                    {
                        if(CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Fire
                            || CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Grass)
                        {
                            return true;
                        }
                        return false;
                    }
                case AppConstants.TipoPokemon.Water:
                    {
                        if (CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Fire
                            || CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Water)
                        {
                            return true;
                        }
                        return false;
                    }
                case AppConstants.TipoPokemon.Grass:
                    {
                        if (CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Grass
                            || CombatManager.Instance.Player.CurrentPokemonPicked.Type == AppConstants.TipoPokemon.Water)
                        {
                            return true;
                        }
                        return false;
                    }
            }
            return false;
        }


        int bestPokemon = -1;
        private bool CheckIfBetterPokemon()
        {

            bool bestPokemonCanKill = false;

            bestPokemon = -1;
            for (int i = 0; i < m_pokemonTeam.Count; i++)
            {
                if(m_pokemonTeam[i] == CurrentPokemonPicked)
                {
                    continue;
                }
                if(m_pokemonTeam[i].Type == CurrentPokemonPicked.Type)
                {
                    continue;
                }

                bool canKill = CalculateIfCanKill(m_pokemonTeam[i]);

                if(m_pokemonTeam[i].Type != AppConstants.TipoPokemon.Normal)
                {   
                    if (m_pokemonTeam[i].Type != CombatManager.Instance.Player.CurrentPokemonPicked.Type)
                    {
                        if (bestPokemon == -1)
                        {
                            if(m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].CurrentHP * 0.5f)
                            {
                                bestPokemon = i;
                                bestPokemonCanKill = canKill;
                            }
                        }
                        else 
                        {
                            if(m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].CurrentHP * 0.5f)
                            {
                                if (bestPokemonCanKill)
                                {
                                    if (!canKill)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].CurrentHP * 0.5f)
                                        {
                                            if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                                && m_pokemonTeam[i].Speed > m_pokemonTeam[bestPokemon].Speed)
                                            {
                                                bestPokemon = i;
                                                bestPokemonCanKill = canKill;
                                            }
                                            else if(m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                                && m_pokemonTeam[i].Speed < m_pokemonTeam[bestPokemon].Speed)
                                            {
                                                if(m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                                {
                                                    bestPokemon = i;
                                                    bestPokemonCanKill = canKill;
                                                }
                                            }
                                            else if(m_pokemonTeam[i].Speed <= CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                            {
                                                if(m_pokemonTeam[i].Type != m_pokemonTeam[bestPokemon].Type)
                                                {
                                                    if(m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
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
                                                    if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                                    {
                                                        bestPokemon = i;
                                                        bestPokemonCanKill = canKill;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if(m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].CurrentHP * 0.5f)
                                    {
                                        if (canKill)
                                        {
                                            bestPokemon = i;
                                            bestPokemonCanKill = canKill;
                                        }
                                        else
                                        {
                                            if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                                && m_pokemonTeam[i].Speed > m_pokemonTeam[bestPokemon].Speed)
                                            {
                                                bestPokemon = i;
                                                bestPokemonCanKill = canKill;
                                            }
                                            else if(m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                                && m_pokemonTeam[i].Speed < m_pokemonTeam[bestPokemon].Speed)
                                            {
                                                if(m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                                {
                                                    bestPokemon = i;
                                                    bestPokemonCanKill = canKill;
                                                }
                                            }
                                            else if(m_pokemonTeam[i].Speed <= CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                            {
                                                if(m_pokemonTeam[i].Type != m_pokemonTeam[bestPokemon].Type)
                                                {
                                                    if (m_pokemonTeam[i].CurrentHP * 1.5f > m_pokemonTeam[bestPokemon].CurrentHP)
                                                    {
                                                        bestPokemon = i;
                                                        bestPokemonCanKill = canKill;
                                                    }
                                                }
                                                else
                                                {
                                                    if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
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
                    }
                }
                else if(m_pokemonTeam[i].Type == AppConstants.TipoPokemon.Normal || m_pokemonTeam[i].Type == CombatManager.Instance.Player.CurrentPokemonPicked.Type)
                {
                    if (bestPokemon == -1)
                    {
                        if(m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].CurrentHP * 0.5f)
                        {
                            bestPokemon = i;
                            bestPokemonCanKill = canKill;
                        }
                    }
                    else
                    {
                        if (m_pokemonTeam[bestPokemon].Type  != AppConstants.TipoPokemon.Normal
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
                                    if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].CurrentHP * 0.5f)
                                    {
                                        if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                            && m_pokemonTeam[i].Speed > m_pokemonTeam[bestPokemon].Speed)
                                        {
                                            bestPokemon = i;
                                            bestPokemonCanKill = canKill;
                                        }
                                        else if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                            && m_pokemonTeam[i].Speed < m_pokemonTeam[bestPokemon].Speed)
                                        {
                                            if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                            {
                                                bestPokemon = i;
                                                bestPokemonCanKill = canKill;
                                            }
                                        }
                                        else if (m_pokemonTeam[i].Speed <= CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                        {
                                            if (m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
                                            {
                                                if (m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
                                                {
                                                    if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP * 0.75f)
                                                    {
                                                        bestPokemon = i;
                                                        bestPokemonCanKill = canKill;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                                {
                                                    bestPokemon = i;
                                                    bestPokemonCanKill = canKill;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (m_pokemonTeam[i].CurrentHP >= m_pokemonTeam[i].CurrentHP * 0.5f)
                                {
                                    if (canKill)
                                    {
                                        bestPokemon = i;
                                        bestPokemonCanKill = canKill;
                                    }
                                    else
                                    {
                                        if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                            && m_pokemonTeam[i].Speed > m_pokemonTeam[bestPokemon].Speed)
                                        {
                                            bestPokemon = i;
                                            bestPokemonCanKill = canKill;
                                        }
                                        else if (m_pokemonTeam[i].Speed >= CombatManager.Instance.Player.CurrentPokemonPicked.Speed
                                            && m_pokemonTeam[i].Speed < m_pokemonTeam[bestPokemon].Speed)
                                        {
                                            if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
                                            {
                                                bestPokemon = i;
                                                bestPokemonCanKill = canKill;
                                            }
                                        }
                                        else if (m_pokemonTeam[i].Speed <= CombatManager.Instance.Player.CurrentPokemonPicked.Speed)
                                        {
                                            if (m_pokemonTeam[bestPokemon].Type == CurrentPokemonPicked.Type)
                                            {
                                                if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP * 0.75f)
                                                {
                                                    bestPokemon = i;
                                                    bestPokemonCanKill = canKill;
                                                }
                                            }
                                            else
                                            {
                                                if (m_pokemonTeam[i].CurrentHP > m_pokemonTeam[bestPokemon].CurrentHP)
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
                }

            }
            if(bestPokemon != -1)
            {
                return false;
            }
            return true;
        }

        private CleverCrow.Fluid.BTs.Tasks.TaskStatus UseBestAttack()
        {
            if (!canKill)
            {
                for (int i = 0; i < CurrentPokemonPicked.Tms.Count; i++)
                {
                    float damageCalc = 0;
                    CombatManager.Instance.OnCalculateDamage?.Invoke(CurrentPokemonPicked.Tms[i], CurrentPokemonPicked, CombatManager.Instance.Player.CurrentPokemonPicked, ref damageCalc);

                    int flooredCalc = Mathf.FloorToInt(damageCalc);
                    if (damageCalc <= flooredCalc)
                    {
                        bestTm = i;
                        damageOfTm = flooredCalc;
                    }
                }
            }

            ChooseAction(CurrentPokemonPicked.Tms[bestTm], CombatManager.ActionType.Attack);

            return TaskStatus.Success;
        }
        private CleverCrow.Fluid.BTs.Tasks.TaskStatus UseHealingItem()
        {
            return TaskStatus.Success;
        }
        private CleverCrow.Fluid.BTs.Tasks.TaskStatus ChangeToBestPokemon()
        {
            if(bestPokemon == -1)
            {
                return TaskStatus.Failure;
            }
            if(m_pokemonTeam[bestPokemon] == m_currentPickedPokemon)
            {
                return TaskStatus.Failure;
            }
            m_changePokemonAction.Pokemon = m_pokemonTeam[bestPokemon];
            ChooseAction(m_changePokemonAction, CombatManager.ActionType.SwapPokemon);
            return TaskStatus.Success;
        }
    }
}

