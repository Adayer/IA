using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRegatea : ActionNode
{
    public override void Execute()
    {
        if (Random.Range(0f, 1f) < 0.5f)
        {
            print(this.transform.root.name + " hace una finta maravillosa y regate al rival");
        }
        else
        {
            print(this.transform.root.name + " intenta regatear al contrario pero solo consigue regatearse a si mismo");
        }
    }
}