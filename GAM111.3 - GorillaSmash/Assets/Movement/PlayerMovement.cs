using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour {
    bool canButtonJump;
    bool canJump = false;
    public float jumpForce = 0.001f;
    Animator gorrilaAnim;
    CapsuleCollider myCapsuleCollider;
    bool isFalling = false;
    bool isJumping = false;
    public float groundedDist = 0f;
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
        gorrilaAnim = GetComponentInChildren<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider>();
        playerRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (!canJump) gorrilaAnim.SetBool("Running", false);
        if (Input.GetAxisRaw("Jump") == 0) {
            canButtonJump = true;
        }
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, myCapsuleCollider.radius - 0.1f, Vector3.down, out hit, myCapsuleCollider.height*0.5f + groundedDist) && isJumping == false) {
            gorrilaAnim.SetBool("Jumped", false);
            currentMoveState = MoveState.OnGround;
            canJump = true;
        }
        else if (isJumping) {
            currentMoveState = MoveState.OnJump;
        }
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
                isFalling = true;
                InAirMove();
                break;
        }
        if (rawInput.magnitude != 0) {
            Vector3 facingDir = input.normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(facingDir, Vector3.up), Time.deltaTime * 5);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }

    void OnGroundMove() {
        MoveGorilla();
        if (rawInput.magnitude != 0) {
            gorrilaAnim.SetBool("Running", true);
        }
        else if (rawInput.magnitude == 0) {
            gorrilaAnim.SetBool("Running", false);
        }
        if (Input.GetAxis("Jump") > 0 && canButtonJump) {
            canButtonJump = false;
            isJumping = true;
            gorrilaAnim.SetBool("Running", false);
        }
    }

    void OnJumpMove() {
        if (canJump) {
            gorrilaAnim.SetBool("Jumped", true);
            playerRB.AddForce(Vector3.up * playerRB.mass * jumpForce);
            canJump = false;
        }
        Invoke("FallTimer", 0.25f);
        if (isJumping == false) {
            currentMoveState = MoveState.InAir;
        }
    }

    void InAirMove() {
        //Vector3 movementVector = Vector3.ClampMagnitude(new Vector3(input.x * airVel, 0, input.z * airVel), airVel);
        // a way to only affect the x and z of .vel
    }

    void MoveGorilla() {
        if (playerRB.velocity.magnitude < maxVel) {
            playerRB.AddForce((playerRB.mass + speed * 2) * rawInput);
        }
        else {
            Vector3.ClampMagnitude(playerRB.velocity, maxVel);
        }
        playerRB.velocity = Vector3.Lerp(playerRB.velocity, new Vector3(0, playerRB.velocity.y, 0), 0.075f);
    }
    void FallTimer() {
        isJumping = false;
    }
}