using System;
using UnityEngine;

public class BossCharacter : EnemyCharacter
{
    private bool primaryProjectile = true;
    private int currentProjectile;
    private int lastAttackWay = 0;

    public void CheckEnemyCount()
    {
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
    }

    public override void Kill()
    {
        base.Kill();
        PoolingSystem.Instance.InstantiateAPS("PlayerExplode", base.transform.position, Quaternion.identity);
    }

    public override void PrimaryAttack()
    {
        if (base.CurrentCooldown <= 0f)
        {
            switch(lastAttackWay % 2)
            {
                case 0:
                    new Danmaku().RoundDanmaku("EnemyProjectile", base.CharacterMesh.position, base.CharacterMesh.rotation.eulerAngles, 30);
                    break;
                case 1:
                    new Danmaku().RoundDanmaku("EnemyProjectilePurple", base.CharacterMesh.position, base.CharacterMesh.rotation.eulerAngles, 30);
                    break;
                /*
                PoolingSystem.Instance.InstantiateAPS(!this.primaryProjectile ? "EnemyProjectilePurple" : "EnemyProjectile", base.CharacterMesh.position, base.CharacterMesh.rotation);
                GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Shoot, -1f);
                this.currentProjectile++;
                if (this.currentProjectile >= 3)
                {
                    this.primaryProjectile = !this.primaryProjectile;
                    this.currentProjectile = 0;
                }
                break;
                */
                default:
                    break;
            }
            lastAttackWay++;
            base.CurrentCooldown = base.Cooldown;
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
    }
}
