using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public bool canMove = true;

    public float actualSpeed = 4f;
    public float walkingSpeed = 4f;
    public float sprintingSpeed = 8f;
    public float crouchingSpeed = 2f;

    public float actualHeight = 3.8f;
    public float walkingHeight = 3.8f;
    public float crouchingHeight = 1.9f;
    public float cameraHeight = 1.58f;

    public float gravity = -9.81f;
    public float jumpHeight = 3;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;


    public bool isCeilingRoom;
    public bool isCrouchingKey;
    public bool isCrouching;
    public bool isSprintingKey;
    public static bool isSprinting;
    public bool isSprintingCheck;

    public float walkingBobAmount = 0.1f;
    public float walkingBobSpeed = 12f;
    public float sprintingBobAmount = 0.4f;
    public float sprintingBobSpeed = 16f;
    public float crouchingBobAmount = 0.05f;
    public float crouchingBobSpeed = 8f;

    public float actualBobAmount;
    public float actualBobSpeed;
    public float bobChange;

    public float bobTimer;

    public float cameraMove;



    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        //Gets Player object for Action Points
        GameObject Player = GameObject.Find("First Person Player");
        APController apController = Player.GetComponent<APController>();

        //Sets controller
        controller = Player.GetComponent<CharacterController>();

        //Sets ground check and mask
        groundCheck = Player.transform.Find("GroundCheck");
        groundMask = LayerMask.GetMask("Ground");

    }

    // Update is called once per frame
    void Update()
    {

        //First check if player can move
        if ( (canMove) ) {

            //Check if player is grounded
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            //Check to see if there is room above the player
            isCeilingRoom = !Physics.Raycast(Camera.main.transform.position, Vector3.up, 1f);

            //Check to see if player is Sprinting/Crouching
            isCrouchingKey = Input.GetButton("Crouch");
            isSprintingKey = Input.GetButton("Sprint");

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
            if( (isSprintingKey) && (moveZ > 0) && (isGrounded) && (APController.CurrentAP > 0f) && !(APController.APWait) )
            {
                actualSpeed = sprintingSpeed;
                isSprinting = true;
                isSprintingCheck = true;
            } else if( (isCrouchingKey) && (isGrounded) )
            {
                actualSpeed = crouchingSpeed;
                isCrouching = true;
            } else { 
                actualSpeed = walkingSpeed;
                isSprinting = false;
                isSprintingCheck = false;
                isCrouching = false;
            }

            //Moves camera if player is crouching;
            if ( ( (isCrouching) && (isGrounded) ) | ( !(isCrouching) && !(isCeilingRoom) ) )
            {
                actualHeight = crouchingHeight;

            } else
            {
                actualHeight = walkingHeight;

            }
            controller.height = Mathf.Lerp(controller.height, actualHeight, 5 * Time.deltaTime);
            controller.center = Vector3.down * (walkingHeight - controller.height) / 2.0f;

            //Calculates Headbob if moving
            if( (isGrounded) && ( (moveX != 0) | (moveZ != 0) ) )
            {
                
                if ( (isSprinting) )
                {
                    actualBobAmount = sprintingBobAmount;
                    actualBobSpeed = sprintingBobSpeed;

                } else if ( (isCrouching) )
                {
                    actualBobAmount = crouchingBobAmount;
                    actualBobSpeed = crouchingBobSpeed;
                    
                } else
                {
                    actualBobAmount = walkingBobAmount;
                    actualBobSpeed = walkingBobSpeed;
                    
                }

                //Constantly iterating timer so that it switches between +ve and -ve for up and down effect
                bobTimer += Time.deltaTime * actualBobSpeed;
                bobChange = Mathf.Sin(bobTimer) * actualBobAmount;
            }  else {
                bobChange = 0;
            }
            
            //Moves camera based on crouching and bobbing
            Camera.main.transform.localPosition = Vector3.down * (walkingHeight - controller.height - cameraHeight - bobChange);

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
}
