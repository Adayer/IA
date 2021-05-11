using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDesmarque : ActionNode
{
    public override void Execute()
    {
        print(this.transform.root.name + " se desmarca a un sitio muy peligroso!");
    }
}

