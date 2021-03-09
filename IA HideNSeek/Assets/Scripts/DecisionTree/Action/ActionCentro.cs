using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCentro : ActionNode
{
    public override void Execute()
    {
        print(this.transform.root.name + " centra al area con peligro y....");
    }
}
