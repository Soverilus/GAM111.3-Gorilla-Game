using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaGet : MonoBehaviour {
    public int bananaAmount;
    public bool superBanana;
    PlayerMovement playerScript;

    private void Start() {
        if (GameObject.FindGameObjectWithTag("Player") != null) {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.GetComponent<PlayerMovement>() != null) {
            playerScript.GetPoints(superBanana, bananaAmount, gameObject);
        }
    }
}
