using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMarcar : ActionNode
{
    public override void Execute()
    {
        print(this.transform.root.name + " llega en el momento justo para marcar al rival y cortar la linea de pase");
    }
}
