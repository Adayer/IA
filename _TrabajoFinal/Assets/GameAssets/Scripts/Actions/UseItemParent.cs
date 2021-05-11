using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemParent : ActionParent
{
    public override IEnumerator Effect()
    {
        Debug.LogError("Item no configurado");
        yield return null;
    }
}
