using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour {
    public int bananasCollected;
    public int superBananasCollected;
    Vector3 originalGravity = Physics.gravity;
    bool canButtonJump;
    bool canJump = false;
    public float jumpForce = 0.001f;
    [HideInInspector]
    public float originalJumpForce;
    Animator gorrilaAnim;
    CapsuleCollider myCapsuleCollider;
    bool isJumping = false;
    public float groundedDist = 0f;
    [HideInInspector]
    public float originalSpeed;
    public float speed;
    [HideInInspector]
    public float originalMaxVel;
    public float maxVel;
    public float groundVel;
    public float airVel;
    public float jumpVel;
    bool inWater;
    float animSpeed;
    //Vector3 inputActual;
    Vector3 rawInputActual;
    //Vector3 previousFacing = Vector3.forward;

    Rigidbody playerRB;
    Quaternion angledOrientation = Quaternion.identity;
    enum MoveState {
        OnGround,
        OnJump,
        InAir
    };
    [SerializeField]
    MoveState currentMoveState = MoveState.InAir;

    //Vector3 input;
    Vector3 rawInput;

    void Start() {
        originalMaxVel = maxVel;
        originalSpeed = speed;
        originalJumpForce = jumpForce;
        gorrilaAnim = GetComponentInChildren<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider>();
        playerRB = GetComponent<Rigidbody>();
        animSpeed = gorrilaAnim.speed;
    }

    void InputCalculation(float inputY) {
        //inputActual = new Vector3(Camera.main.transform.TransformDirection(input).x, inputY, Camera.main.transform.TransformDirection(input).z).normalized;
        rawInputActual = new Vector3(Camera.main.transform.TransformDirection(rawInput).x, inputY, Camera.main.transform.TransformDirection(rawInput).z).normalized;
        //Debug.Log(rawInputActual);
    }

    void FixedUpdate() {
        if (inWater) {
            Physics.gravity = new Vector3(0, originalGravity.y * 0.25f, 0);
            gorrilaAnim.speed = 0.25f * animSpeed;
        }
        else {
            Physics.gravity = originalGravity;
            gorrilaAnim.speed = animSpeed;
        }
        if (!canJump) gorrilaAnim.SetBool("Running", false);
        if (Input.GetAxisRaw("Jump") == 0) {
            canButtonJump = true;
        }
        //input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, myCapsuleCollider.radius - 0.1f, Vector3.down, out hit, myCapsuleCollider.height * 0.5f + groundedDist) && isJumping == false) {
            float dotProd = Vector3.Dot(Vector3.down, hit.normal);
            //Debug.Log(dotProd);
            if (dotProd <= -0.6f) {
                Quaternion oldRotation = transform.rotation;
                transform.up = hit.normal;
                angledOrientation = transform.rotation;
                gorrilaAnim.SetBool("Jumped", false);
                currentMoveState = MoveState.OnGround;
                canJump = true;
                InputCalculation(0f);
                transform.rotation = oldRotation;
            }
            else if (isJumping) {
                currentMoveState = MoveState.OnJump;
                InputCalculation(0f);
            }
            if (dotProd > -0.6f) {
                canJump = false;
                playerRB.AddForce(hit.normal.x * (speed * 1.5f), 250f, hit.normal.z * (speed * 1.5f));

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
                InAirMove();
                break;
        }
        if (rawInputActual.magnitude != 0) {
            //previousFacing = rawInputActual;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(rawInputActual.x, Mathf.Clamp(playerRB.velocity.y, -0.5f, 0.5f), rawInputActual.z)), 0.15F);
        }
        //transform.forward = previousFacing;
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
            playerRB.AddForce((playerRB.mass + speed * 2) * multVel * (angledOrientation * rawInputActual));
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

    public void GetPoints(bool superBanana, int amount, GameObject banana) {
        if (superBanana) {
            superBananasCollected += 1;
        }
        bananasCollected += amount;
        Destroy(banana);
    }

    private void OnTriggerStay(Collider other) {
        GameObject otherGO = other.gameObject;
        if (otherGO.GetComponent<SlowPlayer>() != null) {
            SlowPlayer otherSlow;
            otherSlow = otherGO.GetComponent<SlowPlayer>();
            otherSlow.SlowDown(gameObject);
            inWater = true;
        }
        if (otherGO.GetComponent<OffStage>() != null) {
            /*OffStage otherOffStage;
            otherOffStage = otherGO.GetComponent<OffStage>();*/
        }
    }
    private void OnTriggerExit(Collider other) {
        GameObject otherGO = other.gameObject;
        if (otherGO.GetComponent<SlowPlayer>() != null) {
            speed = originalSpeed;
            maxVel = originalMaxVel;
            jumpForce = originalJumpForce;
            inWater = false;
        }
        if (otherGO.GetComponent<OffStage>() != null) {
            /*OffStage otherOffStage;
            otherOffStage = otherGO.GetComponent<OffStage>();*/
            if (transform.position.y <= otherGO.transform.position.y) {
                IDidntWantAnyBananasAnyways();
            }
        }
    }
    public SceneHolder sceneManager;
    public void IDidntWantAnyBananasAnyways() {
        Time.timeScale = 0f;
        sceneManager.WinLose(false);
        Debug.Log("You've Lost");
    }

    public void TheseAreMyBananasNow() {
        Time.timeScale = 0f;
        sceneManager.WinLose(true);
        Debug.Log("you've won :D");
    }

    public Text banana;
    public Text superBanana;

    private void Update() {
        banana.text = (bananasCollected.ToString() + " :");
        superBanana.text = (superBananasCollected.ToString() + " :");
    }
}