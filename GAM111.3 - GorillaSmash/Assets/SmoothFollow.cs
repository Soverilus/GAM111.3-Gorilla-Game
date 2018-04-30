using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour {
    public float smoothTime;

    public void SmoothCam(Vector3 camVect) {
        Vector3 refRef = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, camVect, ref refRef, smoothTime);
    }
}
