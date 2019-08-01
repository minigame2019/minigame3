using System;
using UnityEngine;

public class EnemyPillar : EnemyCharacter
{
    public Transform[] MultiArmor;
    public Transform[] ProjectileEmitters;

    public override void Movement()
    {
        base.CharacterMesh.Rotate(Vector3.up * base.Stats.TurnSpeed);
    }

    public override void PrimaryAttack()
    {
        if (base.CurrentCooldown <= 0f)
        {
            base.CurrentCooldown = base.Cooldown;
            bool flag = true;
            for (int i = 0; i < this.ProjectileEmitters.Length; i++)
            {
                if (!base.UseBothProjectileTypes)
                {
                    new Danmaku().GrapeshotDanmaku("EnemyProjectile", this.ProjectileEmitters[i].position, this.ProjectileEmitters[i].rotation.eulerAngles, 10, 2);
                    //PoolingSystem.Instance.InstantiateAPS("EnemyProjectile", this.ProjectileEmitters[i].position, this.ProjectileEmitters[i].rotation);
                }
                else
                {
                    PoolingSystem.Instance.InstantiateAPS(!flag ? "EnemyProjectilePurple" : "EnemyProjectile", this.ProjectileEmitters[i].position, this.ProjectileEmitters[i].rotation);
                    flag = !flag;
                }
            }
            //GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Shoot, -1f);
        }
    }

    /* public override void TakeDamage(Transform hitObject)
    {
        
        for (int i = 0; i < this.MultiArmor.Length; i++)
        {
            if (hitObject == this.MultiArmor[i])
            {
                GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Armor, -1f);
                return;
            }
        }
        base.Stats.Health--;
        if (base.Stats.Health <= 0)
        {
            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Explode, -1f);
            this.Kill();
        }
        else
        {
            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Hit, -1f);
            //base.StartCoroutine(base.HitColorChange());
        }
    } */
}
