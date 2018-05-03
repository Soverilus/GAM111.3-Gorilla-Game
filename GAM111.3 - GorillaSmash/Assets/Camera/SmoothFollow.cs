using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour {
    [SerializeField]
    GameObject upCamObj;
    [SerializeField]
    GameObject leftCamObj;
    Vector3 positionHolder;
    GameObject player;
    Vector3 trueInput;
    Vector3 input;
    Vector3 rawInput;
    public float smoothTime;
    Vector3 crossProdInput;
    public float camSensitivity;
    Vector3 heightAbove;

    void Start() {
        heightAbove = Vector3.zero;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (player != null) {
            input = new Vector3(Input.GetAxis("CamHorizontal"), Input.GetAxis("CamVertical"), 0f);
            rawInput = new Vector3(Input.GetAxisRaw("CamHorizontal"), Input.GetAxisRaw("CamVertical"), 0f);

            if (rawInput.magnitude != 0f) {

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
            positionHolder = camVect + heightAbove;
            //}
            transform.position = Vector3.SmoothDamp(transform.position, positionHolder, ref refRef, smoothTime);
            //transform.position = positionHolder;
        }
    }
}
