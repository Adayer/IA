using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPaso : ActionNode
{
    public override void Execute()
    {
        print(this.transform.root.name + " da un pase");
    }
}