using UnityEngine;

public class BioGun : Weapon
{
    [Header("Weapon Settings")]
    public float fireRate = 20f; // 🔹 Disparos por segundo
    public float projectileSpeed = 50f; // 🔹 Velocidad del proyectil
    public AudioClip shootSound; // 🔹 Sonido de disparo
    private AudioSource audioSource; // 🔹 Referencia al AudioSource

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void Shoot(Vector3 direction)
    {
        if (!CanShoot()) return;

        ammo--; 
        lastShotTime = Time.time;

        // 🔹 Instanciar el proyectil
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        BulletProjectile bullet = projectile.GetComponent<BulletProjectile>();

        if (bullet != null)
        {
            bullet.shooterTag = "Player"; // 🔹 Indica que fue disparado por el Player
        }

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed; // 🔹 Usa velocity en lugar de linearVelocity
            rb.useGravity = false; // 🔹 Desactiva la gravedad si no quieres que caiga

            Debug.Log($"🔹 Disparando con velocidad: {rb.linearVelocity.magnitude}");
        }

        // 🔹 Reproducir sonido de disparo
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}