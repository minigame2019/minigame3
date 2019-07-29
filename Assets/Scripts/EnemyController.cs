using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private CharacterBase character;
    private Transform playerTarget;
    [Range(-1f, 1f)]
    public float AttackAngle = 0.5f;
    public float ShotCooldown = 1f;
    private float currentCooldown;
    private Vector2 moveInput;
    private Vector3 lookInput;
    private bool pursueTarget;
    private float waitToAttack = 1f;
    private RaycastHit hit;

    private void FindPlayer()
    {
        this.playerTarget = FindObjectOfType<PlayerCharacter>().transform;
    }

    private void FixedUpdate()
    {
        this.GetInput();
        this.SendInput();
    }

    private void GetInput()
    {
        if (this.playerTarget)
        {
            if (this.pursueTarget || (this.waitToAttack <= 0f))
            {
                this.pursueTarget = true;
            }
            else
            {
                this.waitToAttack -= Time.fixedDeltaTime;
            }
            if (!this.pursueTarget)
            {
                this.moveInput = Vector2.zero;
                this.lookInput = (Vector3)Vector2.zero;
            }
            else
            {
                this.lookInput = (this.playerTarget.position - base.transform.position);
                this.moveInput.x = this.lookInput.x;
                this.moveInput.y = this.lookInput.z;
                this.moveInput = Vector2.ClampMagnitude(this.moveInput, 1f);
            }
            if ((Vector3.Dot(this.character.CharacterMesh.forward, this.lookInput.normalized) > this.AttackAngle) && (Vector3.Distance(base.transform.position, this.playerTarget.position) <= this.character.Stats.AttackRange))
            {
                this.character.PrimaryAttack();
            }
        }
    }

    private void SendInput()
    {
        this.character.ReceiveInput(this.moveInput, this.moveInput);
    }

    private void Start()
    {
        this.waitToAttack = UnityEngine.Random.Range((float)0.5f, (float)1f);
        this.character = base.GetComponent<CharacterBase>();
        this.FindPlayer();
    }
}
