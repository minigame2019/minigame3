using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable<Transform>, IKillable, IMChangeable
{
    public float Speed;
    public bool CanBeDestroyed;
    public float Radius;
    public float Distance;
    public LayerMask CollisionLayers;
    private float distanceThisFrame;
    private float currentDistance;
    private RaycastHit hit;
    private RaycastHit[] hits = new RaycastHit[5];

    private void Awake()
    {
        GameManager.Instance.RegisterProjectile(base.gameObject);
    }

    private void CheckForward()
    {
        if ((this.currentDistance > this.Distance) || !GameManager.Instance.GameRunning())
        {
            this.DestroySelf();
        }
        else
        {
            this.distanceThisFrame = Time.fixedDeltaTime * this.Speed;
            this.currentDistance += this.distanceThisFrame;
            int num = Physics.SphereCastNonAlloc(base.transform.position, this.Radius, base.transform.forward, this.hits, this.distanceThisFrame, (int)this.CollisionLayers, QueryTriggerInteraction.Ignore);
            if (num <= 1)
            {
                if (num != 1)
                {
                    return;
                }
                this.hit = this.hits[0];
            }
            else
            {
                float distanceThisFrame = this.distanceThisFrame;
                for (int i = 0; i < num; i++)
                {
                    if (this.hits[i].distance < distanceThisFrame)
                    {
                        distanceThisFrame = this.hits[i].distance;
                        this.hit = this.hits[i];
                    }
                }
            }
            IDamageable<Transform> component = this.hit.transform.GetComponent<IDamageable<Transform>>();
            Debug.Log(component);
            if (component != null)
            {
                Projectile projectile = this.hit.transform.GetComponent<Projectile>();
                if (projectile && !projectile.CanBeDestroyed)
                {
                    return;
                }
                component.TakeDamage(this.hit.collider.transform);
            }
            //PoolingSystem.Instance.InstantiateAPS("BulletHit", this.hit.point, Quaternion.identity);
            this.DestroySelf();
        }
    }

    public void DestroySelf()
    {
        PoolingSystem.DestroyAPS(base.gameObject);
    }

    private void FixedUpdate()
    {
        this.CheckForward();
        this.MoveForward();
    }

    public void Kill()
    {
        PoolingSystem.Instance.InstantiateAPS("BulletHit", base.transform.position, Quaternion.identity);
        this.DestroySelf();
    }

    private void MoveForward()
    {
        Transform transform = base.transform;
        transform.position += base.transform.forward * this.distanceThisFrame;
    }

    private void OnEnable()
    {
        this.currentDistance = 0f;
    }

    public void TakeDamage(Transform hitObject)
    {
        if (this.CanBeDestroyed)
        {
            this.DestroySelf();
        }
    }

    public void SwitchMaterialToNight(bool isEnteringNight)
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
}
