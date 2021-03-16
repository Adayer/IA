using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAyuda : ActionNode
{
    public override void Execute()
    {
        print(this.transform.root.name + " corre de un lado del campo al otro para dar ayuda a la defensa");
    }
}
