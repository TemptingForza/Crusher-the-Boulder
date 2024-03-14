using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public float damage = 10;
    public string source = "generic";
    public bool onEnter = false;

    void OnTriggerEnter(Collider other)
    {
        if (!onEnter)
            return;
        foreach (var target in other.GetComponents<IDamageable>())
            Attack(target);
    }
    void OnTriggerStay(Collider other)
    {
        if (onEnter)
            return;
        foreach (var target in other.GetComponents<IDamageable>())
            Attack(target);
    }

    void Attack(IDamageable target)
    {
        if (target != null && target.CanDamage(source) && target.OnDamage(damage, source))
            target.Kill();
    }
}
