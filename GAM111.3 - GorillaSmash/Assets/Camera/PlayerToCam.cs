using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToCam : MonoBehaviour {
    [SerializeField]
    GameObject cameraObj;
    SmoothFollow camFol;
    public float camFolRadius;
    //public float followDist;
    public float followMax;
    Vector3 camPos;
    public float followHeight;
    Vector3 heightAbove = new Vector3(0, 10, 0);

    void Start() {
        camFol = cameraObj.GetComponent<SmoothFollow>();
    }
    void Update() {
        RaycastHit hit;
        LayerMask sceneLayer;
        sceneLayer = LayerMask.NameToLayer("Scene");
        if (Physics.SphereCast(new Vector3(transform.position.x, transform.position.y + followHeight, transform.position.z), camFolRadius, (cameraObj.transform.position - transform.position).normalized, out hit, followMax, 1 << sceneLayer.value)) {
            camPos = hit.point + heightAbove;
            camFol.SmoothCam(camPos);
        }
        else {
            Vector3 followDir = transform.position + ((cameraObj.transform.position - transform.position).normalized * followMax);
            camFol.SmoothCam(followDir);
        }

        //cameraObj.transform.position = transform.position + ((cameraObj.transform.position - transform.position).normalized * followMax);

        cameraObj.transform.LookAt(transform);

    }
}
