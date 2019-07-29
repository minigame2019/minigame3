using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private Vector3 Offset = new Vector3(70f, 0f, -7f);
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float shakeMultiplier = 0.1f;
    private static float shakeDuration;
    [SerializeField]
    private Transform tiltArm;
    [SerializeField]
    private Transform camera;

    private void FixedUpdate()
    {
        if (this.Target)
        {
            base.transform.position = Vector3.Lerp(base.transform.position, this.Target.position, Time.fixedDeltaTime * this.moveSpeed);
            if (shakeDuration <= 0f)
            {
                this.camera.transform.localEulerAngles = Vector3.zero;
            }
            else
            {
                shakeDuration -= Time.fixedDeltaTime;
                Transform transform = this.camera.transform;
                transform.localPosition += UnityEngine.Random.insideUnitSphere * this.shakeMultiplier;
                Transform transform2 = this.camera.transform;
                transform2.localEulerAngles += UnityEngine.Random.insideUnitSphere * this.shakeMultiplier;
            }
        }
    }

    public void SetTarget(Transform t)
    {
        this.Target = t;
        base.transform.position = this.Target.position;
        this.camera.transform.localPosition = new Vector3(0f, 0f, this.Offset.z);
        this.tiltArm.transform.localPosition = new Vector3(0f, this.Offset.y, 0f);
        this.tiltArm.transform.localEulerAngles = new Vector3(this.Offset.x, 0f, 0f);
    }

    public static void Shake(float s = 0.04f)
    {
        if (s > shakeDuration)
        {
            shakeDuration = s;
        }
    }

    private void Start()
    {
        Instance = this;
        this.camera = base.GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Shake(0.2f);
        }
    }
}
