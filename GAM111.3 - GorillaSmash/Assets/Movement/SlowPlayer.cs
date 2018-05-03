using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPlayer : MonoBehaviour {
    PlayerMovement targetMovement;

    public void SlowDown(GameObject target) {
        if (target.tag == "Player") {
            targetMovement = target.GetComponent<PlayerMovement>();
        }
        targetMovement.speed = 0.75f * targetMovement.originalSpeed;
        targetMovement.maxVel = 0.25f * targetMovement.originalMaxVel;
        targetMovement.jumpForce = 0.25f * targetMovement.originalJumpForce;
    }
}
