using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Samples
{
    /// <summary>
    /// Example script to test out BehaviorTrees, not actually compiled into the released package
    /// </summary>
    public class EnemyTrainerIA : TrainerParent
    {
        [SerializeField]
        private BehaviorTree _trainerIA;

        //[SerializeField] private bool m_estaCerca = false;
        //[SerializeField] private bool m_soyHarry = false;

        private void Awake()
        {
            _trainerIA = new BehaviorTreeBuilder(gameObject)

            .Build();                       
            #region LoQueYoHice
            //_trainerIA = new BehaviorTreeBuilder(gameObject)
            //.Selector()
            //    .Sequence()
            //        .Condition("Esta cerca?", () => m_estaCerca)
            //        .Selector()
            //            .Sequence()
            //                .Condition("Soy Harry", () => m_soyHarry)
            //                .Do("Expelliermus", () =>
            //                {
            //                    print("Expelliermus");
            //                    return TaskStatus.Success;
            //                })
            //            .End()
            //            .Do("Soy util", () =>
            //            {
            //                print("Lanzo un hechizo util");
            //                return TaskStatus.Success;
            //            })
            //        .End()
            //    .End()
            //        .Selector()
            //            .Sequence()
            //                .Condition("Soy Harry", () => m_soyHarry)
            //                .Do("Grito", () => {
            //                    print("AAAAAAAAAAAAAAAH!");
            //                    return TaskStatus.Success;
            //                })
            //            .End()
            //            .Do("Corro", () =>
            //            {
            //                print("A correr");
            //                return TaskStatus.Success;
            //            })
            //        .End()
            //.End()
            //.Build();
            #endregion

            #region loQueVenia
            //_treeA = new BehaviorTreeBuilder(gameObject)
            //    .Sequence()
            //        .Condition("Peter", () => _condition)                    
            //        .Do("Custom Action A", () => {
            //            print("Cosa");
            //            return TaskStatus.Success;
            //        })
            //        .Selector()
            //            .Sequence("Nested Sequence")
            //                .Condition("Custom Condition", () => _condition)
            //                .Do("Custom Action", () => TaskStatus.Success)
            //            .End()
            //            .Sequence("Nested Sequence")
            //                .Do("Custom Action", () => TaskStatus.Success)
            //                .Sequence("Nested Sequence")
            //                    .Condition("Custom Condition", () => true)
            //                    .Do("Custom Action", () => TaskStatus.Success)
            //                .End()
            //            .End()
            //            .Do("Custom Action", () => TaskStatus.Success)
            //            .Condition("Custom Condition", () => true)
            //        .End()
            //        .Do("Custom Action B", () => TaskStatus.Success)
            //    .End()
            //    .Build();

            //_treeB = new BehaviorTreeBuilder(gameObject)
            //    .Name("Basic")
            //    .Sequence()
            //        .Condition("Custom Condition", () => _condition)
            //        .Do("Custom Action A", () => TaskStatus.Success)
            //    .End()
            //    .Build();

            //_treeC = new BehaviorTreeBuilder(gameObject)
            //    .Name("Basic")
            //    .Sequence()
            //        .Condition("Custom Condition", () => _condition)
            //        .Do("Continue", () => _condition ? TaskStatus.Continue : TaskStatus.Success)
            //    .End()
            //    .Build();
            #endregion
        }

        public void Act()
        {
            m_typeOfActionChosen = CombatManager.ActionType.Attack;
            m_actionChosen = m_currentPickedPokemon.Tm1;

            //_trainerIA.Tick();
        }
    }
}

