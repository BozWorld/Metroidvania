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

public interface IStartCooldown<T>
{
    void StartCooldown(T cooldownT, T cooldown);
}

public interface IUpdateCooldown<T>
{
    void UpdateCooldown(T cooldownT);
}