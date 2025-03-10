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

    protected override void OnShoot()
    {
        // Lógica específica de disparo para SoldierGun
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile does not have a Rigidbody component");
        }
    }
}