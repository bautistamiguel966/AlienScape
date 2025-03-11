using UnityEngine;

public class SoldierProjectile : MonoBehaviour
{
    public int damage = 10; // Daño del proyectil
    public float speed = 20f; // Velocidad del proyectil
    public float lifetime = 3f; // Tiempo de vida del proyectil

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed; // Mueve el proyectil con física
        }
        else
        {
            Debug.LogError("El proyectil del Soldier no tiene un Rigidbody.");
        }

        Destroy(gameObject, lifetime); // Destruir después de un tiempo
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player recibió " + damage + " de daño.");
            }
            else
            {
                Debug.LogError("El Player tiene la etiqueta 'Player' pero no tiene el componente PlayerHealth.");
            }
        }
        else if (collision.gameObject.CompareTag("Soldier"))
        {
            Debug.Log("Impacto con un Soldier, pero sin daño.");
        }
        else
        {
            Debug.Log("Impacto con " + collision.gameObject.name);
        }

        Destroy(gameObject); // Destruir el proyectil tras impactar
    }
}
