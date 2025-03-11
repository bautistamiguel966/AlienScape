using UnityEngine;

public class OrganProjectile : MonoBehaviour
{
    public float explosionRadius = 5f; // Radio de la explosión
    public int explosionDamage = 20; // Daño de la explosión
    public GameObject explosionEffect; // Efecto visual de la explosión (partículas, etc.)

    private void OnCollisionEnter(Collision collision)
    {
        // Lógica de explosión
        Explode();

        // Destruir el proyectil después de la explosión
        Destroy(gameObject);
    }

    private void Explode()
    {
        // Mostrar el efecto visual de la explosión
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Aplicar daño a todos los objetos dentro del radio de explosión
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hitCollider in colliders)
        {
            // Verificar si el objeto tiene un componente de salud (como PlayerHealth)
            PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage);
            }

            // Aquí podrías agregar lógica para dañar otros objetos, como enemigos o estructuras
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el radio de explosión en el editor (solo para debug)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}