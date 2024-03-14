using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour, IDamageable
{
    public float Health = 50;
    public string DestroySound;
    bool IDamageable.CanDamage(string source) => true;
    bool IDamageable.OnDamage(float amount, string source)
    {
        Health -= amount;
        return Health <= 0;
    }
    void IDamageable.Kill() {
        if (DestroySound != null)
            AudioManager.Instance.PlayOneShot(DestroySound, transform.position);
        Destroy(gameObject);
    }
}
