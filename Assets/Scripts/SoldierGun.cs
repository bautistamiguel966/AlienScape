using UnityEngine;

public class SoldierGun : Weapon
{
    public Transform firePointSoldier; // Punto de disparo
    public float projectileSpeed = 20f; // Velocidad del proyectil
    [Range(0f, 1f)] public float accuracy = 0.8f; // Probabilidad de acierto (0 = siempre falla, 1 = siempre acierta)

    protected override void Start()
    {
        firePoint = firePointSoldier;
        base.Start();
    }

    public override void Shoot(Vector3 direction)
    {
        if (PlayerHealth.isPlayerDead) return; // No dispara si el jugador murió
        if (!CanShoot() || firePoint == null || projectilePrefab == null) return;

        ammo--;

        Vector3 finalDirection = ApplyAccuracy(direction);

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(finalDirection));
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = finalDirection * projectileSpeed; // Usar velocity en vez de linearVelocity
        }
        else
        {
            Debug.LogError("El proyectil no tiene un Rigidbody.");
        }
    }

    private Vector3 ApplyAccuracy(Vector3 direction)
    {
        if (accuracy >= 1f) return direction.normalized;

        bool shouldHit = Random.value <= accuracy; // Definir si el disparo es preciso

        if (shouldHit) return direction.normalized;

        // Si falla, aplicar desviación
        float angleDeviation = Random.Range(-10f, 10f);
        Quaternion rotation = Quaternion.Euler(angleDeviation, angleDeviation, 0f);
        return rotation * direction;
    }
}
