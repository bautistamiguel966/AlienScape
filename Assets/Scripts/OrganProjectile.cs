using UnityEngine;

public class OrganProjectile : MonoBehaviour
{
    public float explosionRadius = 5f;
    public int maxDamage = 50;
    public int minDamage = 10;
    public GameObject explosionEffect;
    public float explosionEffectDuration = 2f; // ⏳ Duración del efecto de explosión
    public AudioClip explosionSound; // 🔹 Sonido de la explosión

    private bool hasExploded = false;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasExploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        hasExploded = true;

        // 🔹 Instanciar efecto de explosión y destruirlo tras un tiempo
        if (explosionEffect != null)
        {
            GameObject explosionInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosionInstance, explosionEffectDuration); // 🔹 Destruir el efecto tras 'explosionEffectDuration' segundos
        }

        // 🔹 Crear un objeto temporal para reproducir el sonido
        if (explosionSound != null)
        {
            GameObject soundObject = new GameObject("ExplosionSound");
            AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
            tempAudioSource.clip = explosionSound;
            tempAudioSource.Play();
            Destroy(soundObject, explosionSound.length);
        }
        else
        {
            Debug.LogWarning("⚠ No hay sonido de explosión asignado.");
        }

        // 🔹 Aplicar daño en área
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hitCollider in colliders)
        {
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
            int damage = CalculateDamage(distance);

            if (hitCollider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    Debug.Log($"🔥 Player recibió {damage} de daño por explosión.");
                }
            }

            if (hitCollider.CompareTag("Soldier"))
            {
                SoldierHealth soldierHealth = hitCollider.GetComponent<SoldierHealth>();
                if (soldierHealth != null)
                {
                    soldierHealth.TakeDamage(damage);
                    Debug.Log($"💥 Soldier recibió {damage} de daño por explosión.");
                }
            }
        }

        Destroy(gameObject); // 🔹 Destruir el proyectil tras la explosión
    }

    private int CalculateDamage(float distance)
    {
        if (distance <= 1f) return maxDamage;
        float normalizedDistance = distance / explosionRadius;
        return Mathf.RoundToInt(Mathf.Lerp(maxDamage, minDamage, normalizedDistance));
    }
}
