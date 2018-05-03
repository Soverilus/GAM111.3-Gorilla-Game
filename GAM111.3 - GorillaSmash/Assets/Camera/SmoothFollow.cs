using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour {
    Vector3 positionHolder;
    GameObject player;
    Vector3 trueInput;
    Vector3 input;
    Vector3 rawInput;
    public float smoothTime;
    Vector3 crossProdInput;
    public float camSensitivity;
	Vector3 movementVector;
	Vector3 crossProd;

    void Start() {
		movementVector = Vector3.zero;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
		if (player != null) {
			input = new Vector3 (Input.GetAxis ("CamHorizontal"), Input.GetAxis ("CamVertical"), 0f);
			rawInput = new Vector3 (Input.GetAxisRaw ("CamHorizontal"), Input.GetAxisRaw ("CamVertical"), 0f);

			if (rawInput.magnitude != 0f) {
				if ((Vector3.Dot (Vector3.up, -transform.forward) >= 0.8f && rawInput.y >= 0) || 
					(Vector3.Dot (Vector3.up, -transform.forward) <= -0.8f && rawInput.y <= 0)||
					(Vector3.Dot (Vector3.up, -transform.forward) < 0.8f && Vector3.Dot (Vector3.up, -transform.forward) > -0.8f)) {
					movementVector = (transform.up * -input.y + transform.right * -input.x);
				}
			}
		}
	}
    public void SmoothCam(Vector3 camVect) {
        if (player != null) {
            Vector3 refRef = Vector3.zero;


            //if (input.magnitude > 0) {
            //positionHolder = (camVect - trueInput);
            //}
            //else {
			positionHolder = camVect + movementVector.normalized * camSensitivity;
            //}
            transform.position = Vector3.SmoothDamp(transform.position, positionHolder, ref refRef, smoothTime);
            //transform.position = positionHolder;
			movementVector = Vector3.zero;
        }
    }
}
