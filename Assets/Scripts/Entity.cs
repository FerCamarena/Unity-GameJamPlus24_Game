using UnityEngine;
public class Entity : MonoBehaviour {
    public float health;
    public float baseHealth;

    public float baseRegen;
    public float baseDamage;

    protected void Update() {
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
}