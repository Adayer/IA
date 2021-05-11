using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHueco : ActionNode
{
    public override void Execute()
    {
        print(this.transform.root.name + " se lleva a su defensa y hace hueco");
    }
}
