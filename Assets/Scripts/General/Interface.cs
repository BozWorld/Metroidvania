using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeath
{
    void Death();
}

public interface IDamageable<T>
{
    void Damage(T damageTaken);
}