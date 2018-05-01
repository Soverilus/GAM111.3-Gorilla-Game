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
    public float groundVel;
    public float airVel;
    public float jumpVel;
    Vector3 inputActual;
    Vector3 rawInputActual;
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

    void InputCalculation(float inputY) {
        inputActual = new Vector3(Camera.main.transform.TransformDirection(input).x, inputY, Camera.main.transform.TransformDirection(input).z).normalized;
        rawInputActual = new Vector3(Camera.main.transform.TransformDirection(rawInput).x, inputY, Camera.main.transform.TransformDirection(rawInput).z).normalized;
        Debug.Log(rawInputActual);
    }

    void FixedUpdate() {
        if (!canJump) gorrilaAnim.SetBool("Running", false);
        if (Input.GetAxisRaw("Jump") == 0) {
            canButtonJump = true;
        }
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, myCapsuleCollider.radius - 0.1f, Vector3.down, out hit, myCapsuleCollider.height * 0.5f + groundedDist) && isJumping == false) {
            float dotProd = Vector3.Dot(Vector3.down, hit.normal);
            //Debug.Log(dotProd);
            if (dotProd <= -0.7f) {
                isFalling = true;
                gorrilaAnim.SetBool("Jumped", false);
                currentMoveState = MoveState.OnGround;
                canJump = true;
                InputCalculation(1f - Mathf.Abs(dotProd));
                Debug.Log(1f - Mathf.Abs(dotProd));
            }
            else if (isJumping) {
                currentMoveState = MoveState.OnJump;
                InputCalculation(0f);
            }
            if (dotProd > -0.7f) {
                canJump = false;
                playerRB.AddForce(hit.normal.x * 1000, 100f, hit.normal.z * 1000);
            }
        }
        else if (isJumping) {
            currentMoveState = MoveState.OnJump;
            InputCalculation(0f);
        }
        else {
            currentMoveState = MoveState.InAir;
            InputCalculation(0f);
        }
        

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
            Vector3 facingDir = inputActual;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(facingDir, Vector3.up), Time.deltaTime * 5);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }

    void OnGroundMove() {
        //MoveGorilla(groundVel);
        if (rawInput.magnitude != 0) {
            gorrilaAnim.SetBool("Running", true);
            MoveGorilla(groundVel);
        }
        else if (rawInput.magnitude == 0) {
            gorrilaAnim.SetBool("Running", false);
        }
        if (Input.GetButton("Jump") && canButtonJump) {
            canButtonJump = false;
            isJumping = true;
        }
    }

    void OnJumpMove() {
        MoveGorilla(jumpVel);
        if (canJump) {
            gorrilaAnim.SetBool("Jumped", true);
            gorrilaAnim.SetBool("Running", false);
            playerRB.AddForce(Vector3.up * playerRB.mass * jumpForce);
            canJump = false;
        }
        Invoke("FallTimer", 0.25f);
        if (isJumping == false) {
            currentMoveState = MoveState.InAir;
        }
    }

    void InAirMove() {
        MoveGorilla(airVel);
        //Vector3 movementVector = Vector3.ClampMagnitude(new Vector3(input.x * airVel, 0, input.z * airVel), airVel);
        // a way to only affect the x and z of .vel
    }

    void MoveGorilla(float multVel) {
        if (playerRB.velocity.magnitude < maxVel) {
            playerRB.AddForce((playerRB.mass + speed * 2) * multVel * rawInputActual);
        }
        else {
            Vector3.ClampMagnitude(playerRB.velocity, maxVel);
        }
        if (canButtonJump)
            playerRB.velocity = Vector3.Lerp(playerRB.velocity, new Vector3(0, playerRB.velocity.y, 0), 0.075f);
    }
    void FallTimer() {
        isJumping = false;
    }
}