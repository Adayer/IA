using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOfrezco : ActionNode
{
    public override void Execute()
    {
        print(this.transform.root.name + " se ofrece al pase");
    }
}