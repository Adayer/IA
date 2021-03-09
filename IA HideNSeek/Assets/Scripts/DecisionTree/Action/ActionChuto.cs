using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChuto : ActionNode
{
    public override void Execute()
    {
        if(Random.Range(0f, 1f) < 0.2f)
        {
            print(this.transform.root.name + " chuta y.... GOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOL!!!!");
        }
        else
        {
            print(this.transform.root.name + " chuta y.... FUERAAAA");
        }
    }
}
