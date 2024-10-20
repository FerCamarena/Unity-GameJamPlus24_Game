using UnityEngine;

public class Structure : Entity {
    public Transform closestTarget;

    protected override void Update() {
        base.Update();

        if (closestTarget && Vector3.Distance(transform.position, closestTarget.position) < baseRange && !casting) {
            Attack(); 
        }
    }

    protected override void Attack() {
        if(closestTarget.TryGetComponent<Entity>(out Entity e)) {
            e.TakeHit(baseDamage);

            closestTarget = null;
        }

        StartCoroutine(TryAttack());
    }
    
    private void OnTriggerStay(Collider obj) {
        if (obj.gameObject.layer == 10) {
            if (!closestTarget) closestTarget = obj.transform;
        }
    }
    
    private void OnTriggerEnter(Collider obj) {
        if (obj.gameObject.layer == 10) {
            closestTarget = obj.transform;
        }
    }

    private void OnTriggerExit(Collider obj) {
        if (obj.gameObject.layer == 10) {
            if (closestTarget == obj.transform) closestTarget = null;
        }
    }
}