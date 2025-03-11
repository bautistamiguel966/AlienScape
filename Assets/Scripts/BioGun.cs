using UnityEngine;

public class BioGun : Weapon
{
    public Transform firePointGun; // Punto de disparo del arma
    public float projectileSpeed = 20f;

    protected override void Start()
    {
        firePoint = firePointGun;
        base.Start();
    }

    public override void Shoot(Vector3 direction)
    {
        if (!CanShoot() || firePoint == null || projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        ammo--; // 🔹 Reducir la munición después de disparar
    }
}
