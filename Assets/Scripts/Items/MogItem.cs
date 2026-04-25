using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MogItem : Item
{
    public void ApplyMog(ExplosionAbility script)
    {
        script.AddAmount(amount);
    }
}