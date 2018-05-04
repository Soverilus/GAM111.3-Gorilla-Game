using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollide : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null) {
            PlayerMovement playerScr = collision.gameObject.GetComponent<PlayerMovement>();
            playerScr.TheseAreMyBananasNow();
        }
    }
}
