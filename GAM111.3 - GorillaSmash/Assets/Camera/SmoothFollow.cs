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
    Vector3 heightAbove;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        input = new Vector3(Input.GetAxis("CamHorizontal"), Input.GetAxis("CamVertical"), 0f);
        rawInput = new Vector3(Input.GetAxisRaw("CamHorizontal"), Input.GetAxisRaw("CamVertical"), 0f);
        crossProdInput = Vector3.Cross(player.transform.position, transform.position);
        trueInput = new Vector3(0, crossProdInput.y + input.y, 0f).normalized;
        if (transform.position.y <= player.transform.position.y + 10f) {
            heightAbove = new Vector3(0f, 1f, 0f);
        }
        else {
            heightAbove = Vector3.zero;
        }
    }
    public void SmoothCam(Vector3 camVect) {
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
