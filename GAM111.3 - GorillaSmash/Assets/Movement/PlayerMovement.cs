using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour {
    CapsuleCollider myCapsuleCollider;
    bool isFalling = false;
    bool isJumping = false;
    public float groundedDist = 3.2f;
    public float speed;
    public float maxVel;
    public float airVel;
    public float jumpVel;
    Rigidbody playerRB;
    enum MoveState {
        OnGround,
        OnJump,
        InAir
    };
    [SerializeField]
    MoveState currentMoveState = MoveState.InAir;

    Vector3 input;
    Vector3 rawInput;

    void Start() {
        myCapsuleCollider = GetComponent<CapsuleCollider>();
        playerRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, myCapsuleCollider.radius, Vector3.down, out hit, groundedDist))
            currentMoveState = MoveState.OnGround;
        else if (isJumping) currentMoveState = MoveState.OnJump;
        else currentMoveState = MoveState.InAir;

        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        switch (currentMoveState) {
            case MoveState.OnGround:
                OnGroundMove();
                break;
            case MoveState.OnJump:
                OnJumpMove();
                break;
            case MoveState.InAir:
                InAirMove();
                break;
        }
    }

    void OnGroundMove() {
        if (playerRB.velocity.magnitude < maxVel) {
            playerRB.AddForce((playerRB.mass + speed*2) * rawInput);
        }
        else {
            Vector3.ClampMagnitude(playerRB.velocity, maxVel);
        }


        playerRB.velocity = Vector3.Lerp(playerRB.velocity, new Vector3(0, playerRB.velocity.y, 0), 0.075f);
       // Vector3 normalizedVelocity = playerRB.velocity.normalized;

        //  if (rawInput.x == 0) playerRB.AddForce(-normalizedVelocity.x * speed * Time.fixedDeltaTime, 0, 0);
        // if (rawInput.z == 0) playerRB.AddForce(0, 0, -normalizedVelocity.z * speed * Time.fixedDeltaTime);

        //playerRB.velocity = movementVector;
    }
    void OnJumpMove() {
        //Vector3 movementVector = Vector3.ClampMagnitude(new Vector3(input.x * jumpVel, 0, input.z * jumpVel), jumpVel);
        // a way to only affect the x and z of .vel
        if (input.x != 0 || input.z != 0) {
            playerRB.AddForce(input);
        }
    }
    void InAirMove() {
        isFalling = true;
        //Vector3 movementVector = Vector3.ClampMagnitude(new Vector3(input.x * airVel, 0, input.z * airVel), airVel);
        // a way to only affect the x and z of .vel
        if (input.x != 0 || input.z != 0) {
            playerRB.AddForce(input);
        }
    }
}