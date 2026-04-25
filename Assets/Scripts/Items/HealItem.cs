using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Item
{
    public void ApplyHeal(HealthScript script)
    {
        script.AddHealth(amount);
    }
}