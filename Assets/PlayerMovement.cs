using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float actualSpeed = 4f;
    public float walkingSpeed = 4f;
    public float sprintingSpeed = 8f;
    public float crouchingSpeed = 2f;

    public float normalHeight = 3.8f;
    public float crouchingHeight = 1.9f;
    public float cameraHeight = 1.58f;

    public float gravity = -9.81f;
    public float jumpHeight = 3;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;


    bool isCeilingRoom;
    bool isCrouching;
    bool isSprinting;

    Vector3 velocity;


    // Update is called once per frame
    void Update()
    {

        //First check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Check to see if there is room above the player
        isCeilingRoom = !Physics.Raycast(Camera.main.transform.position, Vector3.up, 1f);

        //Check to see if player is Sprinting/Crouching
        isCrouching = Input.GetButton("Crouch");
        isSprinting = Input.GetButton("Sprint");

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

        //Set speed of movement based on if the sprint/crouch key is being pressed
        if( (isSprinting) && (moveZ > 0) && (isGrounded) )
        {
            actualSpeed = sprintingSpeed;
        } else if( (isCrouching) && (isGrounded) )
        {
            actualSpeed = crouchingSpeed;
        } else { 
            actualSpeed = walkingSpeed;
        }

        //Moves camera if player is crouching;
        if ( (isCrouching) && (isGrounded) )
        {
            controller.height = Mathf.Lerp(controller.height, crouchingHeight, 5 * Time.deltaTime);
            controller.center = Vector3.down * (normalHeight - controller.height) / 2.0f;
            Camera.main.transform.localPosition = Vector3.down * (normalHeight - controller.height - cameraHeight);
            
            
        } else if ( !(isCrouching) && (isCeilingRoom) ) 
        {
            controller.height = Mathf.Lerp(controller.height, normalHeight, 5 * Time.deltaTime);
            controller.center = Vector3.down * (normalHeight - controller.height) / 2.0f;
            Camera.main.transform.localPosition = Vector3.down * (normalHeight - controller.height - cameraHeight);
        } else if ( !(isCrouching) && !(isCeilingRoom) ) 
        {
            controller.height = Mathf.Lerp(controller.height, crouchingHeight, 5 * Time.deltaTime);
            controller.center = Vector3.down * (normalHeight - controller.height) / 2.0f;
            Camera.main.transform.localPosition = Vector3.down * (normalHeight - controller.height - cameraHeight);
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
