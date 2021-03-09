using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAtras : ActionNode
{
    public override void Execute()
    {
        print(this.transform.root.name + " no hace nada interesante");
    }
}
