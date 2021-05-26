using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CleverCrow.Fluid.BTs.Samples;

public class NewTrainerArgs : EventArgs
{
    public EnemyTrainerIA newEnemyTrainer;
    public NewTrainerArgs(EnemyTrainerIA newEnemyTrainer)
    {
        this.newEnemyTrainer = newEnemyTrainer;
    }
}
