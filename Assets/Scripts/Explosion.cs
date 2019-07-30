using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IMChangeable
{
    public void SwitchMaterialToNight(bool isEnteringNight)
    {
        ParticleSystem[] children = GetComponentsInChildren<ParticleSystem>();
        //Material[] mats = GetComponentsInChildren<Material>();
        foreach (var child in children)
        {
            var main = child.main;
            if (isEnteringNight)
            {
                main.startColor = new Color(0f, 0f, 0f);
          //      mats
            }
            else
            {
                main.startColor = new Color(1f, 1f, 1f);
            }
        }
    }
}
