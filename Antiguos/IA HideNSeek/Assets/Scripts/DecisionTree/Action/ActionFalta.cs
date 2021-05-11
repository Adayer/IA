using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFalta : ActionNode
{
    public override void Execute()
    {
        if (Random.Range(0f, 1f) < 0.2f)
        {
            print(this.transform.root.name + " hace una falta perfecta al borde del area  y roba el balón!");
        }
        else
        {
            print(this.transform.root.name + " hace una falta brutal al contrario... Esto podría ser roja!");
        }

    }
}
