using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class PoolingSystemExtensions
{
    public static void DestroyAPS(this GameObject myobject)
    {
        PoolingSystem.DestroyAPS(myobject);
    }

    public static void PlayEffect(this GameObject particleEffect, int particlesAmount)
    {
        PoolingSystem.PlayEffect(particleEffect, particlesAmount);
    }

    public static void PlaySound(this GameObject soundSource)
    {
        PoolingSystem.PlaySound(soundSource);
    }
}
