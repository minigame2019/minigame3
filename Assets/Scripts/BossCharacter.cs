using System;
using UnityEngine;

public class BossCharacter : EnemyCharacter
{
    private bool primaryProjectile = true;
    private int currentProjectile;
    private int lastAttackWay = 0;

    private string defaultProjectile = "EnemyProjectilePurple";
    private string boomerProjuctile = "EnemyProjectileBoomer";

    public void CheckEnemyCount()
    {
        switch (GameManager.Instance.CurrentLevel)
        {
            case 2:
                if(GameManager.Instance.CurrentEnemies == 0)
                {
                    base.gameObject.SetActive(true);
                }
                break;
            case 4:
                if(GameManager.Instance.CurrentEnemies == 1)
                {
                    base.Armor.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }

        /*
        if (GameManager.Instance.CurrentEnemies == 1)
        {
            base.Armor.gameObject.SetActive(false);
        }
        else if (GameManager.Instance.CurrentEnemies <= 3)
        {
            base.Cooldown = 0.15f;
        }
        else if (GameManager.Instance.CurrentEnemies < (GameManager.Instance.TotalEnemies - (((float)GameManager.Instance.TotalEnemies) / 2.5f)))
        {
            base.Cooldown = 0.5f;
        }
        */
    }

    public override void Kill()
    {
        GameManager.Instance.bossIsAlive = false;
        base.Kill();
    }

    public override void PrimaryAttack()
    {
        Vector3 position = base.CharacterMesh.position;
        Quaternion rotation = base.CharacterMesh.rotation;
        Vector3 direction = rotation.eulerAngles;

        switch (GameManager.Instance.CurrentLevel)
        {
            case 2:
                switch (lastAttackWay)
                {
                    case 0:
                    case 30:
                    case 60:
                    case 90:
                        new Danmaku().RoundDanmaku(defaultProjectile, position, direction, 15);
                            break;
                    default:
                        break;
                }
                break;
            case 4:
                switch (lastAttackWay)
                {
                    case 0:
                        PoolingSystem.Instance.InstantiateAPS(boomerProjuctile, position, rotation);
                        break;
                    //case 30:
                    case 90:
                        new Danmaku().GrapeshotDanmaku(defaultProjectile, position, direction, 10, 2);
                        break;
                    default:
                        break;
                        
                }
                break;
            case 6:
                switch (lastAttackWay)
                {
                    case 0:
                    case 60:
                        new Danmaku().RoundDanmaku(defaultProjectile, position, direction, 18);
                        break;
                    default:
                        break;
                }
                break;
            case 7:
                switch (lastAttackWay)
                {
                    case 0:
                    case 60:
                        new Danmaku().RoundDanmaku(boomerProjuctile, position, direction, 45);
                        break;
                    case 10:
                    case 70:
                        new Danmaku().RoundDanmaku(defaultProjectile, position, direction, 15);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        lastAttackWay++;
        if(lastAttackWay >= 120)
        {
            lastAttackWay = 0;
        }
        base.CurrentCooldown = base.Cooldown;

        //GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Shoot, -1f);
        /*
        PoolingSystem.Instance.InstantiateAPS(!this.primaryProjectile ? "EnemyProjectilePurple" : "EnemyProjectile", base.CharacterMesh.position, base.CharacterMesh.rotation);
        GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Shoot, -1f);
        this.currentProjectile++;
        if (this.currentProjectile >= 3)
        {
            this.primaryProjectile = !this.primaryProjectile;
            this.currentProjectile = 0;
        }
        */
    }
    

    public override void VStart()
    {
        base.VStart();
        if(GameManager.Instance.CurrentLevel != 4)
        {
            base.Armor.gameObject.SetActive(false);
        }

        switch (GameManager.Instance.CurrentLevel)
        {
            case 2:
                base.Stats.Health = 30;
                break;
            case 4:
                base.Stats.Health = 20;
                break;
            case 6:
                base.Stats.Health = 15;
                break;
            case 7:
                base.Stats.Health = 50;
                break;
            default:
                break;
        }
    }
}
