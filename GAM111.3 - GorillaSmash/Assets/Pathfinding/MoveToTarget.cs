using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class MoveToTarget : MonoBehaviour {
    [SerializeField]
    GameObject target;

    NavMeshAgent navMeshAgent;

	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
        navMeshAgent.destination = target.transform.position;
    }
    void RunAway() {
        Vector3 runAwayPosition = transform.position + (transform.position - target.transform.position).normalized;
        navMeshAgent.destination = runAwayPosition;
    }
}
