using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DestructableWall : MonoBehaviour, IDamageable<Transform>, IMChangeable
{
    public int Health = 5;
    private Material[] displayMaterials = new Material[0];
    private Color[] displayColors = new Color[0];
    private WaitForSeconds recoilTime = new WaitForSeconds(0.04f);

    private void Start()
    {
        Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
        this.displayMaterials = new Material[componentsInChildren.Length];
        this.displayColors = new Color[componentsInChildren.Length];
        for (int i = 0; i < this.displayMaterials.Length; i++)
        {
            this.displayMaterials[i] = componentsInChildren[i].material;
            this.displayColors[i] = this.displayMaterials[i].color;
        }
    }

    public void TakeDamage(Transform hitObject)
    {
        this.Health--;
        if (this.Health > 0)
        {
            //GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Hit, -1f);
            //base.StartCoroutine(this.HitColorChange());
        }
        else
        {
            //CameraController.Shake(0.1f);
            PoolingSystem.Instance.InstantiateAPS("Explode", base.transform.position, Quaternion.identity);
            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Explode, -1f);
            Destroy(base.gameObject);
        }
    }

    public void SwitchMaterialToNight(bool isEnteringNight)
    {
        foreach (var part in GetComponentsInChildren<MeshRenderer>())
        {
            if (isEnteringNight)
            {
                part.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/CubeLava");
            }
            else
            {
                part.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/CubeIce");
            }
        }
    }
}