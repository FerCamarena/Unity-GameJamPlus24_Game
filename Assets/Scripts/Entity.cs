using UnityEngine;
public abstract class Entity : MonoBehaviour {
    public float health;
    public float baseHealth = 10;

    public float baseRegen = 0.1f;

    public float baseDamage = 1;

    public float baseRange = 2;
    
    protected virtual void Update() {
    }

    protected virtual void Start() {
        health = baseHealth;

        InvokeRepeating("DefaultRegen", 1.0f, 1.0f);
    }

    public void Regenerate(float amount) {
        health += amount;
        if (health > baseHealth) health = baseHealth;
    }

    public void DefaultRegen() {
        health += baseRegen;
        if (health > baseHealth) health = baseHealth;
    }

    public void Die() {
        Destroy(gameObject, 0.1f);
        this.enabled = false;
    }

    public void TakeHit(float amount) {
        health -= amount;
        if (health <= 0) {
            health = 0;
            Die();
        }
    }

    protected abstract void Attack();
}