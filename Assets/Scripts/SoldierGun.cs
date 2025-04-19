using UnityEngine;

public class SoldierGun : Weapon
{
    public float projectileSpeed = 20f;

    public override void Shoot(Vector3 direction)
    {
        if (!CanShoot()) return;

        ammo--;
        lastShotTime = Time.time;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        BulletProjectile bullet = projectile.GetComponent<BulletProjectile>();

        if (bullet != null)
        {
            bullet.shooterTag = "Soldier"; // 🔹 Indica que fue disparado por un enemigo
        }

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
    }
}
