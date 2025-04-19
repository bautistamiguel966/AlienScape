using UnityEngine;

public class OrganThrow : Weapon
{
    public GameObject organProjectileModel; // 🔹 Modelo de la granada en la mano

    public float throwForce = 10f; // Fuerza del lanzamiento
    public float throwAngle = 45f; // Ángulo de disparo en grados
    public AudioClip throwSound; // 🔹 Sonido al lanzar

    private AudioSource sharedAudioSource; // 🔹 Fuente de sonido compartida

    private void Awake()
    {
        sharedAudioSource = FindFirstObjectByType<PlayerController>()?.audioSource;
        if (sharedAudioSource == null)
        {
            Debug.LogError("⚠ No se encontró un AudioSource en PlayerController.");
        }
    }

    public override void Shoot(Vector3 direction)
    {
        if (!CanShoot() || firePoint == null || projectilePrefab == null) return;

        ammo--;
        lastShotTime = Time.time;

        // 🔹 Ocultar la granada en la mano antes de lanzar
        if (organProjectileModel != null)
        {
            organProjectileModel.SetActive(false);
        }

        // 🔹 Instanciar el proyectil
        GameObject organ = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = organ.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false; // 🔹 Asegurar que la física funcione
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // 🔹 Evita que pase a través de objetos

            float angleInRadians = throwAngle * Mathf.Deg2Rad;

            // 🔹 Aplicar un ángulo al lanzamiento
            Vector3 throwDirection = direction.normalized;
            throwDirection.y += Mathf.Tan(angleInRadians);

            rb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("⚠ OrganProjectile no tiene un Rigidbody.");
        }

        // 🔹 Reproducir sonido de lanzamiento usando la fuente de PlayerController
        if (sharedAudioSource != null && throwSound != null)
        {
            sharedAudioSource.PlayOneShot(throwSound);
        }
        else
        {
            Debug.LogWarning("⚠ No se puede reproducir el sonido de lanzamiento.");
        }

        // 🔹 Reactivar la granada en la mano tras un tiempo
        Invoke(nameof(ShowHeldGrenade), 1.5f);
    }

    private void ShowHeldGrenade()
    {
        if (organProjectileModel != null)
        {
            organProjectileModel.SetActive(true);
        }
    }
}
