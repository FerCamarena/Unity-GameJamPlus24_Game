using UnityEngine;

public class Character : Entity {
    
    protected override void Attack() {
        
    }
    
    public override void TakeHit(float amount) {
        base.TakeHit(amount);

        //Here can send UI updates
    }
    
    public override void Die() {
        base.Die();

        //Here can send GameOver updates
    }
}