using System;

public interface IDamageable<T>
{
    void TakeDamage(T hitObject);
}
