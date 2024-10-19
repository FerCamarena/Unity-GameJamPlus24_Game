using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public Transform closestTarget;
    public float closestDistance = 0;
    public List<Transform> allTargets = new ();

    private void Start() {
        StartCoroutine(UpdateTargets());
    }

    private void Update() {
        SeekTarget();
    }

    IEnumerator UpdateTargets() {
        yield return new WaitForSeconds(1);

        if (allTargets.Count > 0) SortTargets();
        else closestDistance = 0;
    }

    private void SortTargets() {
        foreach (Transform target in allTargets) {
            if(Vector3.Distance(transform.localPosition, target.localPosition) < closestDistance || closestDistance == 0) {
                closestDistance = Vector3.Distance(transform.localPosition, target.localPosition);
                closestTarget = target;
            }
        }
    }

    private void SeekTarget() {
        Vector3 direction = Vector3.zero;
        if (closestTarget) direction = closestTarget.localPosition;

        GetComponent<NavMeshAgent>().destination = direction;
    }

    private void OnTriggerEnter(Collider obj) {
        if (obj.gameObject.layer == 8) {
            allTargets.Add(obj.transform);
        }
    }
}