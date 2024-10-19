using UnityEngine;

public class Structure : Entity {
    public Transform closestTarget;
    
    protected override void Attack() {
        if(closestTarget.TryGetComponent<Entity>(out Entity e)) {
            e.TakeHit(baseDamage);
        }
    }
    
    private void OnTriggerStay(Collider obj) {
        if (obj.gameObject.layer == 8) {
            if (!closestTarget) closestTarget = obj.transform;
        }
    }
    
    private void OnTriggerEnter(Collider obj) {
        if (obj.gameObject.layer == 8) {
            closestTarget = obj.transform;
        }
    }

    private void OnTriggerExit(Collider obj) {
        if (obj.gameObject.layer == 8) {
            if (closestTarget == obj.transform) closestTarget = null;
        }
    }
}