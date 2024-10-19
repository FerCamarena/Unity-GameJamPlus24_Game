using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class Enemy : Entity {
    public Transform closestTarget;
    public List<Transform> allTargets = new ();

    protected override void Start() {
        GetComponent<NavMeshAgent>().stoppingDistance = baseRange - 0.5f;

        InvokeRepeating("UpdateTargets", 1.0f, 1.0f);
    }

    protected override void Update() {
        base.Update();

        SeekTarget();
    }

    protected override void Attack() {
        if(closestTarget.TryGetComponent<Entity>(out Entity e)) {
            e.TakeHit(baseDamage);
        }
    }

    private void UpdateTargets() {
        if (allTargets.Count > 0) SortTargets();
    }

    private void SortTargets() {
        float newDistance = 15.0f;
        foreach (Transform target in allTargets) {
            if (newDistance > Vector3.Distance(transform.position, target.position)) {
                newDistance = Vector3.Distance(transform.position, target.position);
                closestTarget = target;
            }
        }
    }

    private void SeekTarget() {
        Vector3 direction = Vector3.zero;

        if (closestTarget) {
            direction = closestTarget.localPosition;
            if (Vector3.Distance(transform.position, closestTarget.position) < baseRange) {
                Attack();
            }
        }

        GetComponent<NavMeshAgent>().destination = direction;

    }
    
    private void OnTriggerStay(Collider obj) {
        if (obj.gameObject.layer == 8) {
            if (!allTargets.Contains(obj.transform)) {
                allTargets.Add(obj.transform);
            }
        }
    }

    private void OnTriggerExit(Collider obj) {
        if (obj.gameObject.layer == 8) {
            if (allTargets.Contains(obj.transform)) {
                allTargets.Remove(obj.transform);
            }
        }
    }
}