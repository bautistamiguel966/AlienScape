using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Salud máxima del jugador
    private int currentHealth; // Salud actual del jugador

    private void Start()
    {
        // Inicializar la salud al máximo al comenzar
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Reducir la salud del jugador
        currentHealth -= damage;

        // Verificar si el jugador ha muerto
        if (currentHealth <= 0)
        {
            Die();
        }

        Debug.Log("Jugador recibió " + damage + " de daño. Salud actual: " + currentHealth);
    }

    private void Die()
    {
        // Lógica para cuando el jugador muere
        Debug.Log("Jugador ha muerto");
        // Aquí puedes agregar lógica adicional, como reiniciar el nivel o mostrar una pantalla de Game Over.
    }
}