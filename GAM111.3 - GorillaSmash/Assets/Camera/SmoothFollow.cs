using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour {
    GameObject player;
    Vector3 input;
    Vector3 rawInput;
    public float smoothTime;

    void Start() {
        GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        input = new Vector3(Input.GetAxis("CamHorizontal"), Input.GetAxis("CamVertical"), 0f);
        rawInput = new Vector3(Input.GetAxisRaw("CamHorizontal"), Input.GetAxisRaw("CamVertical"), 0f);
        if (rawInput.x > 0) {
            transform.position += -transform.right * Time.deltaTime * input.x *10f;
        }
        if (rawInput.x < 0) {
            transform.position += transform.right * Time.deltaTime * -input.x * 10f;
        }
        if (rawInput.y > 0) {
            transform.position += -transform.up * Time.deltaTime * input.y * 10f;
        }
        if (rawInput.y < 0) {
            transform.position += transform.up * Time.deltaTime * -input.y * 10f;
        }
    }

    public void SmoothCam(Vector3 camVect) {
        Vector3 refRef = Vector3.zero;
        //transform.position = Vector3.SmoothDamp(transform.position, camVect, ref refRef, smoothTime);
        transform.position = camVect;
    }
}
