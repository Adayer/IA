﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNada : ActionNode
{
    public override void Execute()
    {
        print(this.transform.root.name + " no hace nada de interes");
    }
}
