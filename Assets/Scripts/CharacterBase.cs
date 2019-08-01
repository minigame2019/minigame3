using System;
using UnityEngine;

public class CharacterBase : MonoBehaviour, IDamageable<Transform>, IKillable, IMChangeable
{
    public UnityEngine.Rigidbody Rigidbody;
    public Transform CharacterMesh;
    public CharacterStats Stats;
    public float Cooldown = 0.05f;
    protected float CurrentCooldown;
    protected Vector3 MovementInput;
    protected Vector3 LookInput;

    public bool canMove = true;

    public virtual void CooldownTimers()
    {
    }

    private void FixedUpdate()
    {
        this.Movement();
    }

    public virtual void GetComponents()
    {
        this.Rigidbody = base.GetComponent<UnityEngine.Rigidbody>();
    }

    public virtual void Kill()
    {
        CameraController.Shake(0.1f);
        Destroy(base.gameObject);
    }

    public virtual void Movement()
    {
    }

    public virtual void PrimaryAttack()
    {
    }

    public virtual void SwitchMaterialToNight(bool isEnteringNight)
    {
        foreach (var part in GetComponentsInChildren<MeshRenderer>())
        {
            if (isEnteringNight)
            {
                part.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/White");
            }
            else
            {
                part.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/DarkGrey");
            }
        }
    }

    public void ReceiveInput(Vector2 move, Vector2 look)
    {
        this.MovementInput.x = move.x;
        this.MovementInput.z = move.y;
        this.LookInput.x = look.x;
        this.LookInput.z = look.y;
    }

    private void Start()
    {
        this.GetComponents();
        this.VStart();
    }

    public virtual void TakeDamage(Transform hitObject)
    {
    }

    private void Update()
    {
        this.CooldownTimers();
    }

    public virtual void VStart()
    {
    }

    [Serializable]
    public class CharacterStats
    {
        public int Health;
        public float MoveSpeed;
        public float TurnSpeed;
        public float AttackRange;
    }
}
