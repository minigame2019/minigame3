using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHunter : EnemyCharacter
{
    public void OnCollisionEnter(Collision other)
    {
        PlayerCharacter player = other.rigidbody.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            TakeDamage(this.Jour[0]);
            player.TakeDamage(other.transform);
        }
    }
}
