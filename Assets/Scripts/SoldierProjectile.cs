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
        // Verificar si el proyectil colisiona con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtener el componente PlayerHealth del jugador
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Aplicar daño al jugador
                playerHealth.TakeDamage(damage);
                Debug.Log("Player recibió daño: " + damage);
            }
            else
            {
                Debug.LogError("El objeto con tag 'Player' no tiene el componente PlayerHealth");
            }
        }

        // Verificar si el proyectil colisiona con un Soldier (y evitar que se dañe a sí mismo)
        else if (collision.gameObject.CompareTag("Soldier"))
        {
            Debug.Log("El proyectil colisionó con un Soldier, pero no se aplicará daño.");
        }

        // Destruir el proyectil al colisionar
        Destroy(gameObject);
    }
}