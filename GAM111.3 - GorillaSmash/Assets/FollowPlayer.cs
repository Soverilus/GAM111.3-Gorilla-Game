﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    GameObject player;
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update () {
        transform.position = player.transform.position;
	}
}
