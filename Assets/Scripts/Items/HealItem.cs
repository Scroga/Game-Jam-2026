using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Item
{
    public void ApplyHeal(HealthScript script)
    {
        SoundManager.Instance.PlaySound2D("Heal");
        script.AddHealth(amount);
    }
}