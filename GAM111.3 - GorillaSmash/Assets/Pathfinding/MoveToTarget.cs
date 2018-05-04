using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveToTarget : MonoBehaviour {
    GameObject player;

    NavMeshAgent navMeshAgent;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (Vector3.Distance(transform.position, player.transform.position) <= 100f) {
            navMeshAgent.destination = player.transform.position;
        }
        else {
            navMeshAgent.destination = transform.position;
        }
    }
    /*void RunAway() {
        Vector3 runAwayPosition = transform.position + (transform.position - target.transform.position).normalized;
        navMeshAgent.destination = runAwayPosition;
    }*/
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject == player) {
            player.GetComponent<PlayerMovement>().IDidntWantAnyBananasAnyways();
        }
    }
}
