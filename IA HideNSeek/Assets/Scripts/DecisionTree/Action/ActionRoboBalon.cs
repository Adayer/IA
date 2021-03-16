using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRoboBalon : ActionNode
{
    public override void Execute()
    {
        if (Random.Range(0f, 1f) < 0.2f)
        {
            print(this.transform.root.name + " consigue robar el balón al contrario y comienza un contraataque");
        }
        else
        {
            print(this.transform.root.name + " intenta robar el balón pero es sorteado con facilidad por el contrario");
        }

    }
}

