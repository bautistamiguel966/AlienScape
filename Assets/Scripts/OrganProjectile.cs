using UnityEngine;

public class OrganProjectile : MonoBehaviour
{
    public float explosionRadius = 5f; // Radio de la explosión
    public int maxDamage = 50; // Daño máximo en el centro de la explosión
    public int minDamage = 10; // Daño mínimo en el borde del radio
    public GameObject explosionEffect; // Efecto visual de la explosión

    private Rigidbody rb;
    private bool hasExploded = false; // Para evitar múltiples explosiones

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 🔹 Asegurar que el Rigidbody tenga gravedad activada
        if (rb != null)
        {
            rb.isKinematic = false; // Ahora cae con la gravedad
            rb.useGravity = true; // Asegurar que la gravedad está activada
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 🔹 Si ya explotó, ignorar
        if (hasExploded) return;

        // 🔹 Solo explotar si choca contra el suelo o un enemigo
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Soldier"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        hasExploded = true;

        // 🔹 Mostrar efecto de explosión
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 🔹 Aplicar daño en área
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hitCollider in colliders)
        {
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
            int damage = CalculateDamage(distance);

            // 🔹 Aplicar daño a Player
            PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"🔥 Player recibió {damage} de daño por explosión.");
            }

            // 🔹 Aplicar daño a Soldiers
            SoldierHealth soldierHealth = hitCollider.GetComponent<SoldierHealth>();
            if (soldierHealth != null)
            {
                soldierHealth.TakeDamage(damage);
                Debug.Log($"💥 Soldier recibió {damage} de daño por explosión.");
            }
        }

        // 🔹 Destruir el proyectil después de explotar
        Destroy(gameObject);
    }

    private int CalculateDamage(float distance)
    {
        if (distance <= 1f) return maxDamage;
        float normalizedDistance = distance / explosionRadius;
        return Mathf.RoundToInt(Mathf.Lerp(maxDamage, minDamage, normalizedDistance));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
