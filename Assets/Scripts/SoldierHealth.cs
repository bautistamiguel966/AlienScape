using UnityEngine;

public class SoldierHealth : MonoBehaviour
{
    public int maxHealth = 100; // Salud máxima del Soldier
    private int currentHealth; // Salud actual del Soldier

    private void Start()
    {
        // Inicializar la salud al máximo al comenzar
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Reducir la salud del Soldier
        currentHealth -= damage;

        // Verificar si el Soldier ha muerto
        if (currentHealth <= 0)
        {
            Die();
        }

        Debug.Log(gameObject.name + " recibió " + damage + " de daño. Salud actual: " + currentHealth);
    }

    private void Die()
    {
        // Lógica para cuando el Soldier muere
        Debug.Log(gameObject.name + " ha muerto");

        // Aquí puedes agregar lógica adicional, como:
        // - Reproducir una animación de muerte
        // - Soltar algún objeto (como munición o salud)
        // - Desactivar el NavMeshAgent y otros componentes
        // - Destruir el objeto después de un tiempo

        // Destruir el Soldier
        Destroy(gameObject);
    }
}