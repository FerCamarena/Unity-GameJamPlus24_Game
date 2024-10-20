using System.Collections.Generic;
using UnityEngine;

public class Structure : Entity {
    public Transform closestTarget;
    public List<Transform> allTargets = new();

    public GameObject displaySpawn;

    protected override void Start() {
        base.Start();

        InvokeRepeating("UpdateTargets", 1.0f, 1.0f);
    }

    protected override void Update() {
        base.Update();
        
        if (closestTarget && Vector3.Distance(transform.position, closestTarget.position) < baseRange && !casting) {
            Attack(); 
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
        allTargets.RemoveAll(item => item == null);
        foreach (Transform target in allTargets) {
            if (target && newDistance >= Vector3.Distance(transform.position, target.position)) {
                newDistance = Vector3.Distance(transform.position, target.position);
                closestTarget = target;
            }
        }
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