using UnityEngine;

public class BioProjectile : MonoBehaviour
{
    public int damage = 15; // Daño que hace el proyectil (puede ser diferente al de SoldierProjectile)
    public float speed = 20f; // Velocidad del proyectil
    public float lifetime = 3f; // Tiempo de vida del proyectil

    private void Start()
    {
        // Destruir el proyectil después de un tiempo
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Mover el proyectil hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si el proyectil colisiona con el jugador
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        // Verificar si el proyectil colisiona con un Soldier
        SoldierHealth soldierHealth = collision.gameObject.GetComponent<SoldierHealth>();
        if (soldierHealth != null)
        {
            soldierHealth.TakeDamage(damage);
        }

        // Destruir el proyectil al colisionar
        Destroy(gameObject);
    }
}