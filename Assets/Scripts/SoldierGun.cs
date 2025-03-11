using UnityEngine;

public class SoldierGun : Weapon
{
    public Transform firePointSoldier; // Punto de disparo específico para SoldierGun
    public float projectileSpeed = 20f; // Velocidad del proyectil

    protected override void Start()
    {
        // Asignar el firePoint específico para SoldierGun
        firePoint = firePointSoldier;

        // Llamar al Start de la clase base para verificar asignaciones
        base.Start();
    }

    public void ShootTowards(Vector3 direction)
    {
        if (CanShoot())
        {
            if (firePoint == null || projectilePrefab == null)
            {
                Debug.LogError("FirePoint or ProjectilePrefab is not assigned in " + GetType().Name);
                return;
            }

            ammo--; // Reducir la munición
            OnShootTowards(direction);
        }
    }

    protected void OnShootTowards(Vector3 direction)
    {
        // Instanciar el proyectil y dispararlo en la dirección especificada
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile does not have a Rigidbody component");
        }
    }
}