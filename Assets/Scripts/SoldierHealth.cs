using System.Collections;
using UnityEngine;

public class SoldierHealth : MonoBehaviour
{

    public int maxHealth = 50; // Salud máxima del Soldier
    private int currentHealth; // Salud actual del Soldier

    
    private Animator anim;
    
    private void Start()
    {
        // Inicializar la salud al máximo al comenzar
        currentHealth = maxHealth;
        // 🔹 obtener animator del hijo
        anim = GetComponentInChildren<Animator>();
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
    Debug.Log(gameObject.name + " ha muerto");

    // Desactivar el disparo 
    GetComponent<SoldierGun>().enabled = false;

    // Desactivar movimiento
    GetComponent<SoldierController>().enabled = false; // Si tiene un script propio de movimiento

    anim.SetTrigger("Death");
    // Obtener la duración de la animación
    float tiempoAnimacion = anim.GetCurrentAnimatorStateInfo(0).length+20;

    // Destruir después de que termine la animación
    Destroy(gameObject, tiempoAnimacion);
}



}