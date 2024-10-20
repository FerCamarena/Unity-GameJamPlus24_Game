using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

public class Ally : Entity {
    public Transform closestTarget;
    public List<Transform> allTargets = new ();

    public bool thinking = false;
    public Vector3 patrolDir = Vector3.zero;

    public GameObject displaySpawn;

    protected override void Start() {
        GetComponent<NavMeshAgent>().stoppingDistance = baseRange - 1;

        InvokeRepeating("UpdateTargets", 1.0f, 1.0f);

        InvokeRepeating("DirectPatrol", 1.0f, 1.0f);
    }

    protected override void Update() {
        base.Update();

        if (closestTarget) SeekTarget();
        else GetComponent<NavMeshAgent>().destination = patrolDir;
    }

    public void DirectPatrol() {
        if (!thinking && !closestTarget) {
            patrolDir = transform.position - new Vector3(Random.Range(-5, 5), 0.0f, Random.Range(-5, 5));
            StartCoroutine(CDMovement());
        }
    }

    protected override void Attack() {
        if(closestTarget.TryGetComponent<Entity>(out Entity e)) {
            e.TakeHit(baseDamage);
            if (e.health <= baseDamage) allTargets.Remove(closestTarget);
        }

        StartCoroutine(TryAttack());
    }

    private void UpdateTargets() {
        if (allTargets.Count > 0) SortTargets();
    }

    private void SortTargets() {
        float newDistance = 15.0f;
        foreach (Transform target in allTargets) {
            if (target && newDistance >= Vector3.Distance(transform.position, target.position)) {
                newDistance = Vector3.Distance(transform.position, target.position);
                closestTarget = target;
            }
        }
    }

    private IEnumerator CDMovement() {
        thinking = true;
        yield return new WaitForSecondsRealtime(baseCD);
        thinking = false;
    }

    private void SeekTarget() {
        Vector3 direction = Vector3.zero;

        direction = closestTarget.localPosition;
        if (Vector3.Distance(transform.position, closestTarget.position) < baseRange && !casting) {
            Attack(); 
        }

        GetComponent<NavMeshAgent>().destination = direction;
    }

    private void OnTriggerStay(Collider obj) {
        if (obj.gameObject.layer == 10) {
            if (!allTargets.Contains(obj.transform)) {
                allTargets.Add(obj.transform);
            }
        }
    }

    private void OnTriggerExit(Collider obj) {
        if (obj.gameObject.layer == 10) {
            if (allTargets.Contains(obj.transform)) {
                allTargets.Remove(obj.transform);
            }
        }
    }

    public override void Die() {
        base.Die();

        if (displaySpawn) {
            Destroy(displaySpawn);
        }
    }
}