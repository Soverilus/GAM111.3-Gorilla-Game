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
	
	// Update is called once per frame
	void Update () {
        Vector3 runAwayPosition = transform.position + (transform.position - target.transform.position).normalized;
        //navMeshAgent.Move((transform.position - target.transform.position).normalized);
        navMeshAgent.destination = runAwayPosition;
        //navMeshAgent.destination = (transform.position - target.transform.position).normalized;
    }
}
