using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iDamageable
{
    void TakeDamage(int damage, Vector3 fromDirection);
}
