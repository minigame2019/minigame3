using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyCharacter : CharacterBase
{
    public Transform Armor;
    public bool isBoss;

    public Transform[] Jour;  // white part, can be destroyed at night
    public Transform[] Nuit;  // black part, can be destroyed at daytime

    public bool UseBothProjectileTypes;
    private Material[] displayMaterials = new Material[0];
    private Color[] displayColors = new Color[0];
    private WaitForSeconds recoilTime = new WaitForSeconds(0.04f);

    public override void CooldownTimers()
    {
        if (base.CurrentCooldown > 0f)
        {
            base.CurrentCooldown -= Time.deltaTime;
        }
    }

    public override void Kill()
    {
        //CameraController.Shake(0.1f);
        GameManager.Instance.RemoveEnemy();
        PoolingSystem.Instance.InstantiateAPS("Explode", base.transform.position, Quaternion.identity);
        Destroy(base.gameObject);
    }

    public override void Movement()
    {
        base.Rigidbody.velocity = base.CharacterMesh.forward * (base.MovementInput.magnitude * base.Stats.MoveSpeed);
        if (base.LookInput.sqrMagnitude > 0f)
        {
            Quaternion a = Quaternion.LookRotation(base.LookInput);
            if (Quaternion.Angle(a, base.CharacterMesh.rotation) > 1f)
            {
                base.CharacterMesh.rotation = Quaternion.Lerp(base.CharacterMesh.rotation, a, Time.fixedDeltaTime * base.Stats.TurnSpeed);
            }
        }
        else if (base.MovementInput.sqrMagnitude > 0f)
        {
            Quaternion a = Quaternion.LookRotation(base.MovementInput);
            if (Quaternion.Angle(a, base.CharacterMesh.rotation) > 1f)
            {
                base.CharacterMesh.rotation = Quaternion.Lerp(base.CharacterMesh.rotation, a, Time.fixedDeltaTime * base.Stats.TurnSpeed);
            }
        }
    }

    public override void PrimaryAttack()
    {
        if (base.CurrentCooldown <= 0f)
        {
            base.CurrentCooldown = base.Cooldown;
            string itemType = !this.UseBothProjectileTypes ? "EnemyProjectile" : ((UnityEngine.Random.Range(0, 100) <= 50) ? "EnemyProjectilePurple" : "EnemyProjectile");
            PoolingSystem.Instance.InstantiateAPS(itemType, base.CharacterMesh.position, base.CharacterMesh.rotation);
            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Shoot, -1f);
        }
    }

    public override void TakeDamage(Transform hitObject)
    {
        if(GameManager.Instance.CurrentLevel == 6 && !this.isBoss)
        {
            return;
        }

        if (hitObject == this.Armor)
        {
            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Armor, -1f);
        }
        else
        {
            for (int i = 0; i < this.Jour.Length; i++)
            {
                if (hitObject == this.Jour[i])
                {
                    if (GameManager.Instance.isAtNight)
                    {
                        base.Stats.Health--;
                        if (base.Stats.Health <= 0)
                        {
                            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Explode, -1f);
                            this.Kill();
                        }
                        else
                        {
                            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Hit, -1f);
                            //base.StartCoroutine(this.HitColorChange());
                        }
                    }
                    else
                    {
                        GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Armor, -1f);
                    }
                }
            }


            for (int i = 0; i < this.Nuit.Length; i++)
            {
                if (hitObject == this.Nuit[i])
                {
                    if (!GameManager.Instance.isAtNight)
                    {
                        base.Stats.Health--;
                        if (base.Stats.Health <= 0)
                        {
                            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Explode, -1f);
                            this.Kill();
                        }
                        else
                        {
                            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Hit, -1f);
                            //base.StartCoroutine(this.HitColorChange());
                        }
                    }
                    else
                    {
                        GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Armor, -1f);
                    }
                    return;
                }
            }
            /*
            base.Stats.Health--;
            if (base.Stats.Health <= 0)
            {
                GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Explode, -1f);
                this.Kill();
            }
            else
            {
                GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Hit, -1f);
                //base.StartCoroutine(this.HitColorChange());
            }
            */
        }
    }

    public override void VStart()
    {
        GameManager.Instance.AddEnemy();
        Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
        this.displayMaterials = new Material[componentsInChildren.Length];
        this.displayColors = new Color[componentsInChildren.Length];
        for (int i = 0; i < this.displayMaterials.Length; i++)
        {
            this.displayMaterials[i] = componentsInChildren[i].material;
            //this.displayColors[i] = this.displayMaterials[i].color;
        }
    }
}
