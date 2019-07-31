using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbsorber : EnemyCharacter
{
    public int energy;

    public override void Kill()
    {
        base.Kill();
    }

    public override void PrimaryAttack()
    {
        base.PrimaryAttack();
    }

    public override void TakeDamage(Transform hitObject)
    {
        if(hitObject == this.Armor)
        {
            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Armor, -1f);
            return;
        }

        if (!GameManager.Instance.isAtNight)
        {
            new Danmaku().RoundDanmaku("EnemyProjectileBoomer", this.transform.position, this.transform.rotation.eulerAngles, 45);
        }

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
    }

    public override void SwitchMaterialToNight(bool isEnteringNight)
    {
        /*
        base.SwitchMaterialToNight(isEnteringNight);
        Debug.Log("here");
        if (isEnteringNight)
        {
            Debug.Log("to night");
            Debug.Log(this.energy);
            if(this.energy > 4)
            {
                new Danmaku().RoundDanmaku("EnemyProjectileBoomer", this.transform.position, this.transform.rotation.eulerAngles, 45);
            }
            else if(this.energy > 0)
            {
                new Danmaku().RoundDanmaku("EnemyPrijectilePurple", this.transform.position, this.transform.rotation.eulerAngles, 15);
            }
            this.energy = 0;
        }
        */
    }

}
