using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CleverCrow.Fluid.BTs.Samples;

public class NewTrainerArgs : EventArgs
{
    public TrainerSO newEnemyTrainer;
    public NewTrainerArgs(TrainerSO newEnemyTrainer)
    {
        this.newEnemyTrainer = newEnemyTrainer;
    }
}
