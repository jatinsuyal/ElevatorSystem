using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    public CharacterController cc;
    public bool groundedPlayer;
    Vector3 playerVelocity;
    public float gravityValue = -10f;
    public float playerSpeed = 5f;
    public float xVal, yVal;

    float forwardSpeedMultiplier = 1f;
    Vector3 movementValue = Vector3.zero;
    public bool canJump = true;

    void Update()
    {
        playerMovementFn();

        playerRotationFn();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed();
        }

    }

    public bool isworking;
    void playerMovementFn()
    {
        if (isworking)
        {
            groundedPlayer = cc.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                jumpPointer = jumpAllowed;
                playerVelocity.y = 0f;
            }
            if (groundedPlayer)
            {
                xVal = Input.GetAxis("Horizontal");
                yVal = Input.GetAxis("Vertical");
            }

            Vector2 dir = new Vector2(xVal, yVal);
            dir = dir.normalized;

            if (cc.enabled)
            {
                if (groundedPlayer) movementValue = transform.right * dir.x + (transform.forward * dir.y) * forwardSpeedMultiplier;
                cc.Move(movementValue * Time.deltaTime * playerSpeed);

                playerVelocity.y += gravityValue * Time.deltaTime;
                cc.Move(playerVelocity * Time.deltaTime);//just for gravity
            }
        }
    }
    float rotX, rotY;

    public void playerRotationFn()
    {

        rotX = Input.GetAxis("Mouse X") * 1;
        rotY = Input.GetAxis("Mouse Y") * 1;

        transform.Rotate(0, rotX, 0);
    }

    public bool isFpp = false;

    public int jumpAllowed = 1;
    int jumpPointer;
    public float jumpHeight = 10f;

    public void jumpPressed()
    {
        if (!canJump) return;

        if (jumpPointer == 1)
        {
            jumpPointer -= 1;

            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }
}
