using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterBase character;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Transform lockOnTarget;
    private List<Transform> targetsWithinLockOnRange = new List<Transform>();
    private bool useController = true;

    private void GetInput()
    {
        if ((!GameManager.Instance.GamePaused() && GameManager.Instance.GameRunning()) && (Input.GetButtonDown("Fire2")))
        {
            this.character.Stats.MoveSpeed /= 3;
        }
        if ((!GameManager.Instance.GamePaused() && GameManager.Instance.GameRunning()) && (Input.GetButtonUp("Fire2")))
        {
            this.character.Stats.MoveSpeed *= 3;
        }

        if ((!GameManager.Instance.GamePaused() && GameManager.Instance.GameRunning()) && (Input.GetButton("Fire1") || (Input.GetAxis("Fire1") > 0.5f)))
        {
            this.character.PrimaryAttack();
        }
        this.moveInput.x = Input.GetAxis("Horizontal");
        this.moveInput.y = Input.GetAxis("Vertical");

        this.moveInput = Vector2.ClampMagnitude(this.moveInput, 1f);
        this.lookInput = Vector2.zero;

        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, 1000, 1 << 12))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;
            this.lookInput.x = playerToMouse.x;
            this.lookInput.y = playerToMouse.z;
            // Ensure the vector is entirely along the floor plane.
        }
    }

    private void SendInput()
    {
        this.character.ReceiveInput(this.moveInput, this.lookInput);
    }

    private void Start()
    {
        this.character = base.GetComponent<CharacterBase>();
    }

    private void Update()
    {
        this.GetInput();
        this.SendInput();
    }
}
