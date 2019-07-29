using System;
using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    public GameObject HealthPiece1;
    public GameObject HealthPiece2;
    public GameObject HealthPiece3;

    public override void CooldownTimers()
    {
        if (base.CurrentCooldown > 0f)
        {
            base.CurrentCooldown -= Time.deltaTime;
        }
    }

    public override void Kill()
    {
        GameManager.Instance.StageFailed();
        PoolingSystem.Instance.InstantiateAPS("PlayerExplode", base.transform.position, Quaternion.identity);
        base.Kill();
    }

    public override void Movement()
    {
        base.Rigidbody.velocity = Vector3.Lerp(base.Rigidbody.velocity, base.MovementInput * base.Stats.MoveSpeed, Time.fixedDeltaTime * 15f);
        // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
        if (!LookInput.Equals(Vector3.zero))
        {
            Quaternion newRotatation = Quaternion.LookRotation(LookInput);
            // Set the player's rotation to this new rotation.
            Rigidbody.MoveRotation(newRotatation);
        }
    }

    public override void PrimaryAttack()
    {
        if (base.CurrentCooldown <= 0f)
        {
            base.CurrentCooldown = base.Cooldown;
            PoolingSystem.Instance.InstantiateAPS("Projectile", base.CharacterMesh.position, base.CharacterMesh.rotation);
            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Shoot, -1f);
        }
    }

    public override void TakeDamage(Transform hitObject)
    {
        CameraController.Shake(0.1f);
        base.Stats.Health--;
        this.HealthPiece3.SetActive(base.Stats.Health >= 3);
        this.HealthPiece2.SetActive(base.Stats.Health >= 2);
        this.HealthPiece1.SetActive(base.Stats.Health >= 1);
        if (base.Stats.Health <= 0)
        {
            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Explode, -1f);
            this.Kill();
        }
        else
        {
            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Explode, -1f);
            Collider[] colliderArray = Physics.OverlapSphere(base.transform.position, 4f, (int)GameManager.Instance.ProjectileLayer);
            for (int i = 0; i < colliderArray.Length; i++)
            {
                IKillable component = colliderArray[i].attachedRigidbody.GetComponent<IKillable>();
                if (component != null)
                {
                    component.Kill();
                }
            }
            PoolingSystem.Instance.InstantiateAPS("PlayerExplode", base.transform.position, Quaternion.identity);
        }
    }

    public override void VStart()
    {
        CameraController.Instance.SetTarget(base.transform);
    }
}
