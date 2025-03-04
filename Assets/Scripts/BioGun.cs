using UnityEngine;

public class BioGun : Weapon
{
    public Transform firePointGun; // Punto de disparo del arma biológica
    public float projectileSpeed = 20f;

    protected override void Start()
    {
        // Asignar el firePoint específico para BioGun
        firePoint = firePointGun;

        // Llamar al Start de la clase base para verificar asignaciones
        base.Start();
    }

    protected override void OnShoot()
    {
        // Lógica específica de disparo para BioGun
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