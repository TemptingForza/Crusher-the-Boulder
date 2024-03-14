using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    bool CanDamage(string source);
    bool OnDamage(float amount, string source);
    void Kill();
}
