using UnityEngine;

public class SoldierProjectile : MonoBehaviour
{
    public int damage = 10; // Daño que hace el proyectil
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
        if (collision.gameObject.CompareTag("Player"))
        {
            // Si el proyectil colisiona con el jugador, aplicar daño
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        // Destruir el proyectil al colisionar
        Destroy(gameObject);
    }
}