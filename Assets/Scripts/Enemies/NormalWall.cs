using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NormalWall : MonoBehaviour, IMChangeable
{
    public void SwitchMaterialToNight(bool isEnteringNight)
    {
        foreach (var part in GetComponentsInChildren<MeshRenderer>())
        {
            if (isEnteringNight)
            {
                part.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/DarkGrey");
            }
            else
            {
                part.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/White");
            }
        }
    }
}