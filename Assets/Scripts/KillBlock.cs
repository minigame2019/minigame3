using System;
using UnityEngine;

public class KillBlock : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        PlayerCharacter component = other.transform.root.GetComponent<PlayerCharacter>();
        if (component)
        {
            component.TakeDamage(other.transform);
        }
    }
}
