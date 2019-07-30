using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHunter : EnemyCharacter
{
    public void OnCollisionEnter(Collision other)
    {
        PlayerCharacter player = other.rigidbody.GetComponent<PlayerCharacter>();
        Debug.Log(other);
        if (player != null)
        {
            Kill();
            player.TakeDamage(other.transform);
        }
    }
}
