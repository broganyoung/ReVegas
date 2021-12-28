using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float actualSpeed = 2f;
    public float walkingSpeed = 2f;
    public float sprintingSpeed = 12f;

    public float gravity = -9.81f;
    public float jumpHeight = 3;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {

        //First check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //If player is grounded then set their velocity to -2 (so it's properly reset)
        if( (isGrounded) && (velocity.y < 0f) )
        {
            velocity.y = -2f;
        }

        //Set the X and Z movements of the frame
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        //Creates movement based on where we want to go and where we are facing
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        //Set speed of movement based on if the sprint key is being pressed
        if( (Input.GetButton("Sprint")) && (moveZ > 0) && (isGrounded) )
        {
            actualSpeed = sprintingSpeed;
        } else
        {
            actualSpeed = walkingSpeed;
        }

        //Moves the player
        controller.Move(move * actualSpeed * Time.deltaTime);

        //If the Jump button is pressed the adds velocity
        if( (Input.GetButtonDown("Jump")) && (isGrounded) )
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Adds gravity to velocity
        velocity.y += gravity * Time.deltaTime;

        //Moves player based on velocity
        controller.Move(velocity * Time.deltaTime);
    }
}
