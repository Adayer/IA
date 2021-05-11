using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCorre : ActionNode
{
    public override void Execute()
    {
        print(this.transform.root.name + " sigue controlando el balon");
    }
}
